// onotseike@hotmail.comPaula Aliu
using System;
namespace WebRTC.Servers.Interfaces
{
    public interface IExecutorService : IExecutor
    {
        void Release();
    }
}
