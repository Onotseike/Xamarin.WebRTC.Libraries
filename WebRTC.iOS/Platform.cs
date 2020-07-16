// onotseike@hotmail.comPaula Aliu
using System;
using System.Collections.Generic;
using System.Linq;
using WebRTC.iOS.Binding;

namespace WebRTC.iOS
{
    public static class Platform
    {
        public static void Init(IDictionary<string, string> trialsFields = null, bool enableInternalTracer = true)
        {
            if (trialsFields?.Any() ?? false)
            {
                RTCFieldTrials.InitFieldTrialDictionary(trialsFields);
            }

            if (enableInternalTracer)
            {
                //RTCTracing.RTCStartInternalCapture("log.cs");
            }

            RTCSSLAdapter.RTCInitializeSSL();
            Classes.NativeFactory.Init(new NativeFactory());
        }

        public static void Cleanup()
        {
            RTCTracing.RTCShutdownInternalTracer();
            RTCSSLAdapter.RTCCleanupSSL();
        }
    }
}
