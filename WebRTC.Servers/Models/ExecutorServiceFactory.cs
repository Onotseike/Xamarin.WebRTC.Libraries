// onotseike@hotmail.comPaula Aliu
using System;

using WebRTC.Servers.Interfaces;

namespace WebRTC.Servers.Models
{
    public static class ExecutorServiceFactory
    {
        public static IExecutor HeadExecutor { get; set; }
        public static IExecutorService CreateExecutorService(string tag) => new ExecuteService(tag);
    }
}
