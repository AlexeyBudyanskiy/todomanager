using System;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace ToDoManager.WEB.Infrastructure.TransientFaultHandling
{
    public static class RetryPolicyFactory
    {
        public static RetryPolicy MakeExponentialHttpRetryPolicy(int count = 10, bool notFoundIsTransient = false)
        {
            var strategy = new HttpTransientErrorDetectionStrategy(notFoundIsTransient);
            return Exponential(strategy, count);
        }

        public static RetryPolicy MakeIncrementalHttpRetryPolicy(int count = 10,
                                                int initialIntervalInSeconds = 0,
                                                int intervalInSeconds = 1)
        {
            var initial = TimeSpan.FromSeconds(initialIntervalInSeconds);
            var interval = TimeSpan.FromSeconds(intervalInSeconds);

            var strategy = new HttpTransientErrorDetectionStrategy(true);
            var incrementalPolicy = new Incremental(count, initial, interval);
            return new RetryPolicy(strategy, incrementalPolicy);
        }

        private static RetryPolicy Exponential(ITransientErrorDetectionStrategy stgy,
                                                int retryCount = 10,
                                                double maxBackoffDelayInSeconds = 1024,
                                                double delta = 2)
        {
            var maxBackoff = TimeSpan.FromSeconds(maxBackoffDelayInSeconds);
            var deltaBackoff = TimeSpan.FromSeconds(delta);
            var minBackoff = TimeSpan.FromSeconds(0);

            var exponentialBackoff = new ExponentialBackoff(retryCount,
                                                            minBackoff,
                                                            maxBackoff,
                                                            deltaBackoff);
            return new RetryPolicy(stgy, exponentialBackoff);
        }
    }
}
