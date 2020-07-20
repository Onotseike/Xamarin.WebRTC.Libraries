// onotseike@hotmail.comPaula Aliu
using System;
namespace WebRTC.Servers.Interfaces
{
    public interface IExecutor
    {
        bool IsCurrentExecutor { get; }
        void Execute(Action action);
    }
}
