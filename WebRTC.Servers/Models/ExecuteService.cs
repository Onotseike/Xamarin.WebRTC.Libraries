// onotseike@hotmail.comPaula Aliu
using System;
using System.Collections.Concurrent;
using System.Threading;

using WebRTC.Servers.Interfaces;

namespace WebRTC.Servers.Models
{
    internal class ExecuteService : IExecutorService
    {
        private readonly BlockingCollection<Action> jobs = new BlockingCollection<Action>();
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly Thread thread;
        private readonly string tag;

        public bool IsCurrentExecutor => thread == Thread.CurrentThread;

        public ExecuteService(string _tag)
        {
            tag = _tag;
            thread = new Thread(OnStart)
            {
                IsBackground = true
            };
            thread.Start();
        }

        private void OnStart(object obj)
        {
            try
            {
                using (jobs)
                {
                    foreach (var job in jobs.GetConsumingEnumerable(cancellationTokenSource.Token))
                    {
                        Console.WriteLine("Executing JOB - {0}", tag);
                        job();
                    }
                }
            }
            catch (OperationCanceledException)
            {

            }
        }

        public void Execute(Action action)
        {
            jobs.Add(action);
        }

        public void Release()
        {
            cancellationTokenSource.Cancel();
        }
    }
}