namespace MDA.Disruptor
{
    public interface ICursored
    {
        /// <summary>
        /// Get the current cursor value.
        /// </summary>
        long GetCursor();
    }
}
