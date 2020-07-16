// onotseike@hotmail.comPaula Aliu
using System;
using WebRTC.Interfaces;

namespace WebRTC.Classes
{
    public abstract class NativeObjectBase : INativeObject
    {

        protected NativeObjectBase()
        {
        }

        protected NativeObjectBase(object nativeObject)
        {
            NativeObject = nativeObject;
        }



        public object NativeObject { get; protected set; }

        public virtual void Dispose()
        {
            if (NativeObject == null)
                throw new NullReferenceException();
            if (NativeObject is IDisposable disposable)
                disposable.Dispose();
        }
    }
}
