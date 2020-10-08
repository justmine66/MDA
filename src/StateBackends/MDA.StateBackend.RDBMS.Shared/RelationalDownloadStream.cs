﻿using System;
using System.Data.Common;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.StateBackend.RDBMS.Shared
{
    /// <summary>
    /// This is a chunked read implementation for ADO.NET providers which do
    /// not otherwise implement <see cref="DbDataReader.GetStream(int)"/> natively.
    /// </summary>
    public class RelationalDownloadStream : Stream
    {
        /// <summary>
        /// A cached task as if there are multiple rounds of reads, it is likely
        /// the bytes read is the same. This saves one allocation.
        /// </summary>
        private Task<int> _lastTask;

        /// <summary>
        /// The reader to use to read from the database.
        /// </summary>
        private DbDataReader _reader;

        /// <summary>
        /// The position in the overall stream.
        /// </summary>
        private long _position;

        /// <summary>
        /// The column ordinal to read from.
        /// </summary>
        private readonly int _ordinal;

        /// <summary>
        /// The total number of bytes in the column.
        /// </summary>
        private readonly long _totalBytes;

        /// <summary>
        /// The internal byte array buffer size used in .CopyToAsync.
        /// This size is just a guess and is likely dependent on the database
        /// tuning settings (e.g. read_buffer_size in case of MySQL).
        /// </summary>
        private const int InternalReadBufferLength = 4092;

        /// <summary>
        /// The default constructor.
        /// </summary>
        /// <param name="reader">The reader to use to read from the database.</param>
        /// <param name="ordinal">The column ordinal to read from.</param>
        public RelationalDownloadStream(DbDataReader reader, int ordinal)
        {
            _reader = reader;
            _ordinal = ordinal;

            //This return the total length of the column pointed by the ordinal.
            _totalBytes = reader.GetBytes(ordinal, 0, null, 0, 0);
        }

        /// <summary>
        /// Can the stream be read.
        /// </summary>
        public override bool CanRead => (_reader != null) && (!_reader.IsClosed);

        /// <summary>
        /// Are seeks supported.
        /// </summary>
        /// <remarks>Returns <em>FALSE</em>.</remarks>
        public override bool CanSeek => false;

        /// <summary>
        /// Can the stream timeout.
        /// </summary>
        /// <remarks>Returns <em>FALSE</em>.</remarks>
        public override bool CanTimeout => true;

        /// <summary>
        /// Can the stream write.
        /// </summary>
        /// <remarks>Returns <em>FALSE</em>.</remarks>
        public override bool CanWrite => false;

        /// <summary>
        /// The length of the stream.
        /// </summary>        
        public override long Length => _totalBytes;

        /// <summary>
        /// The current position in the stream.
        /// </summary>
        public override long Position
        {
            get => _position;
            set => throw new NotSupportedException();
        }


        /// <summary>
        /// Throws <exception cref="NotSupportedException"/>.
        /// </summary>        
        /// <exception cref="NotSupportedException" />.
        public override void Flush() => throw new NotSupportedException();

        /// <summary>
        /// Reads the stream.
        /// </summary>
        /// <param name="buffer">The buffer to read to.</param>
        /// <param name="offset">The offset to the buffer to stat reading.</param>
        /// <param name="count">The count of bytes to read to.</param>
        /// <returns>The number of actual bytes read from the stream.</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            //This will throw with the same parameter names if the parameters are not valid.
            ValidateReadParameters(buffer, offset, count);

            try
            {
                var length = Math.Min(count, (int)(_totalBytes - _position));
                long bytesRead = 0;
                if (length > 0)
                {
                    bytesRead = _reader.GetBytes(_ordinal, _position, buffer, offset, length);
                    _position += bytesRead;
                }

                return (int)bytesRead;
            }
            catch (DbException dex)
            {
                //It's not OK to throw non-IOExceptions from a Stream.
                throw new IOException(dex.Message, dex);
            }
        }

        /// <summary>
        /// Reads the stream.
        /// </summary>
        /// <param name="buffer">The buffer to read to.</param>
        /// <param name="offset">The offset to the buffer to stat reading.</param>
        /// <param name="count">The count of bytes to read to.</param>        
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of actual bytes read from the stream.</returns>
        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            //This will throw with the same parameter names if the parameters are not valid.
            ValidateReadParameters(buffer, offset, count);

            if (cancellationToken.IsCancellationRequested)
            {
                var tcs = new TaskCompletionSource<int>();
                tcs.SetCanceled();
                return tcs.Task;
            }

            try
            {
                //The last used task is saved in order to avoid one allocation when the number of bytes read
                //will likely be the same multiple times.
                var bytesRead = Read(buffer, offset, count);
                var ret = _lastTask != null && bytesRead == _lastTask.Result 
                    ? _lastTask 
                    : (_lastTask = Task.FromResult(bytesRead));

                return ret;
            }
            catch (Exception e)
            {
                //Due to call to Read, this is for sure a IOException and can be thrown out.
                var tcs = new TaskCompletionSource<int>();
                tcs.SetException(e);

                return tcs.Task;
            }
        }

        /// <summary>
        /// A buffer copy operation from database to the destination stream.
        /// </summary>
        /// <param name="destination">The destination stream.</param>
        /// <param name="bufferSize">The buffer size.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <remarks>Reading from the underlying ADO.NET provider is currently synchro</remarks>
        public override async Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                var buffer = new byte[InternalReadBufferLength];
                int bytesRead;
                while ((bytesRead = Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Throws <exception cref="NotSupportedException"/>.
        /// </summary>
        /// <param name="offset">The offset to the stream.</param>
        /// <param name="origin">The origin.</param>
        /// <returns>Throws <exception cref="NotSupportedException"/>.</returns>
        /// <exception cref="NotSupportedException" />.
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

        /// <summary>
        /// Throws <exception cref="NotSupportedException"/>. 
        /// </summary>
        /// <returns>Throws <exception cref="NotSupportedException"/>.</returns>
        /// <exception cref="NotSupportedException" />.
        public override void SetLength(long value) => throw new NotSupportedException();

        /// <summary>
        /// Throws <exception cref="NotSupportedException"/>. 
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset to the buffer.</param>
        /// <param name="count">The count of bytes to read.</param>
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        /// <summary>
        /// Dispose.
        /// </summary>
        /// <param name="disposing">Whether is disposing or not.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _reader = null;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Checks the parameters passed into a ReadAsync() or Read() are valid.
        /// </summary>        
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        private static void ValidateReadParameters(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }
            try
            {
                if (checked(offset + count) > buffer.Length)
                {
                    throw new ArgumentException("Invalid offset length");
                }
            }
            catch (OverflowException)
            {
                throw new ArgumentException("Invalid offset length");
            }
        }
    }
}
