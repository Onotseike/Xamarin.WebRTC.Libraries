// onotseike@hotmail.comPaula Aliu
using System;
using Org.Webrtc;
using WebRTC.Enums;

namespace WebRTC.Android.Extensions
{
    public static class ScalingTypeExtensions
    {
        public static RendererCommon.ScalingType ToNative(this ScalingType self)
        {
            switch (self)
            {
                case ScalingType.AspectFit:
                    return RendererCommon.ScalingType.ScaleAspectFit;
                case ScalingType.AspectFill:
                    return RendererCommon.ScalingType.ScaleAspectFill;
                case ScalingType.AspectBalanced:
                    return RendererCommon.ScalingType.ScaleAspectBalanced;
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }
    }
}
