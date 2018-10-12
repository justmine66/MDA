using System;

namespace MDA.Disruptor
{
    /// <summary>
    /// <see cref="ISequenceBarrier"/> handed out for gating <see cref="IEventProcessor"/>s on a cursor sequence and optional dependent <see cref="IEventProcessor"/>(s), using the given WaitStrategy.
    /// </summary>
    public class ProcessingSequenceBarrier : ISequenceBarrier
    {
        private AbstractSequencer abstractSequencer;
        private IWaitStrategy waitStrategy;
        private ISequence cursor;
        private ISequence[] sequencesToTrack;

        public ProcessingSequenceBarrier(AbstractSequencer abstractSequencer, IWaitStrategy waitStrategy, ISequence cursor, ISequence[] sequencesToTrack)
        {
            this.abstractSequencer = abstractSequencer;
            this.waitStrategy = waitStrategy;
            this.cursor = cursor;
            this.sequencesToTrack = sequencesToTrack;
        }

        public bool IsAlerted => throw new NotImplementedException();

        public void Alert()
        {
            throw new NotImplementedException();
        }

        public void CheckAlert()
        {
            throw new NotImplementedException();
        }

        public void ClearAlert()
        {
            throw new NotImplementedException();
        }

        public long GetCursor()
        {
            throw new NotImplementedException();
        }

        public long WaitFor(long sequence)
        {
            throw new NotImplementedException();
        }
    }
}
