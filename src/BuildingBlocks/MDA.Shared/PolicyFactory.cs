using System;
using Polly;

namespace MDA.Shared
{
    public class PolicyFactory
    {
        public static Policy CreateRetry(int retryCount, Action<Exception, TimeSpan, int> action, TimeUnit timeUnit = TimeUnit.Minute)
        {
            Func<int, TimeSpan> functor;
            switch (timeUnit)
            {
                case TimeUnit.Second:
                    functor = index => TimeSpan.FromSeconds(Math.Pow(2, index));
                    break;
                default:
                    functor = index => TimeSpan.FromMinutes(Math.Pow(2, index));
                    break;
            }

            var policy = Policy.Handle<Exception>()
                .WaitAndRetry(retryCount, functor,
                    (exception, duration, index, context) => action(exception, duration, index));

            return policy;
        }

        public static AsyncPolicy CreateAsyncRetry(int retryCount, Action<Exception, TimeSpan, int> action, TimeUnit timeUnit = TimeUnit.Minute)
        {
            Func<int, TimeSpan> functor;
            switch (timeUnit)
            {
                case TimeUnit.Second:
                    functor = index => TimeSpan.FromSeconds(Math.Pow(2, index));
                    break;
                default:
                    functor = index => TimeSpan.FromMinutes(Math.Pow(2, index));
                    break;
            }

            var policy = Policy.Handle<Exception>()
                .WaitAndRetryAsync(retryCount, functor,
                    (exception, duration, index, context) => action(exception, duration, index));

            return policy;
        }

        /// <summary>
        /// 时间单元
        /// </summary>
        public enum TimeUnit
        {
            Minute, Second
        }
    }
}