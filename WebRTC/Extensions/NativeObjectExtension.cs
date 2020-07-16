// onotseike@hotmail.com
//Paula Aliu


using System;
using WebRTC.Interfaces;

namespace WebRTC.Extensions
{
    public static class NativeObjectExtension
    {
        public static T ToNative<T>(this INativeObject nativeObject) => (T)nativeObject.NativeObject;

    }
}
