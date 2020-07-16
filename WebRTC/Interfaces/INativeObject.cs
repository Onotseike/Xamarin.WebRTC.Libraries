// onotseike@hotmail.comPaula Aliu
using System;
namespace WebRTC.Interfaces
{
    public interface INativeObject : IDisposable
    {
        object NativeObject { get; }
    }
}
