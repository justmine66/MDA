﻿namespace MDA.Disruptor
{
    public interface IBatchStartAware
    {
        void OnBatchStart(long batchSize);
    }
}
