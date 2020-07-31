// onotseike@hotmail.comPaula Aliu
using System;
namespace WebRTC.RTC.Abstraction
{
    public interface IExecutor
    {
        #region Method(s)

        bool IsCurrentExecutor { get; }
        void Execute(Action _action);

        #endregion
    }

    public interface IExecutorService : IExecutor
    {
        #region Method(s)

        void Release();

        #endregion
    }
}
