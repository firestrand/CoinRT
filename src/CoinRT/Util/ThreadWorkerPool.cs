using System;
using System.Threading;
using System.Threading.Tasks;

using CoinRT.Common;
using MetroLog;

namespace CoinRT.Util
{
    /// <summary>
    /// Uses worker threads from main thread pool, but has local thread limit.
    /// </summary>
    public interface IThreadWorkerPool
    {
        bool IsFull { get; }
        void Shutdown();
        bool TryAllocateWorker(Func<CancellationToken, Task> action);
    }

    /// <summary>
    /// Uses worker threads from main thread pool, but has local thread limit.
    /// </summary>
    public class ThreadWorkerPool : IThreadWorkerPool
    {
        private static readonly ILogger Log = Logger.GetLoggerForDeclaringType();

        private readonly SemaphoreSlim threadCount;

        private readonly CancellationTokenSource cts = new CancellationTokenSource();

        public bool IsFull
        {
            get { return threadCount.CurrentCount == 0; }
        }

        public ThreadWorkerPool(int maxThreads)
        {
            threadCount = new SemaphoreSlim(maxThreads, maxThreads);
        }

        public void Shutdown()
        {
            cts.Cancel();
        }

        public bool TryAllocateWorker(Func<CancellationToken, Task> action)
        {
            // Shutdown in progress;
            if (cts.IsCancellationRequested)
            {
                return false;
            }

            // Return immediately if the pool is full.
            if (!threadCount.Wait(0))
            {
                return false;
            }

            StartWorker(action);
            return true;
        }

        private async void StartWorker(Func<CancellationToken, Task> action)
        {
            try
            {
                await Task.Run(async () => await action(cts.Token), cts.Token);
            }
            catch (Exception ex)
            {
                Log.Error("Unhandled exception in worker thread", ex);
            }

            threadCount.Release();
        }
    }
}
