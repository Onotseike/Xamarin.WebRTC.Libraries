// onotseike@hotmail.comPaula Aliu
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace WebRTC.RTC.Abstraction
{
    public static class ExecutorServiceFactory
    {
        public static IExecutor MainExecutor { get; set; }
        public static IExecutorService CreateExecutorService(string tag) => new ExecuteService(tag);


        private class ExecuteService : IExecutorService
        {
            private readonly BlockingCollection<Action> _jobs = new BlockingCollection<Action>();
            private readonly CancellationTokenSource _cts = new CancellationTokenSource();
            private readonly Thread _thread;
            private readonly string _tag;

            public ExecuteService(string tag)
            {
                _tag = tag;
                _thread = new Thread(OnStart)
                {
                    IsBackground = true
                };
                _thread.Start();
            }

            public bool IsCurrentExecutor => _thread == Thread.CurrentThread;
            public void Execute(Action action)
            {
                _jobs.Add(action);
            }

            public void Release()
            {
                _cts.Cancel();
            }

            private void OnStart()
            {
                try
                {
                    using (_jobs)
                    {
                        foreach (var job in _jobs.GetConsumingEnumerable(_cts.Token))
                        {
                            Console.WriteLine("Executing job - {0}", _tag);
                            job();
                        }
                    }
                }
                catch (OperationCanceledException)
                {

                }
            }
        }
    }
}
