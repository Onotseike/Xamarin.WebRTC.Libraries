// onotseike@hotmail.comPaula Aliu
using Android.App;
using Org.Webrtc;
namespace WebRTC.Android
{
    public static class Platform
    {
        public static void Init(Activity context, string trialsFields = null, bool enabledInternalTracer = true)
        {
            Init(context.Application, trialsFields, enabledInternalTracer);
        }

        public static void Init(Application application, string trialsFields = null, bool enabledInternalTracer = true)
        {
            var options = PeerConnectionFactory.InitializationOptions.InvokeBuilder(application)
                .SetEnableInternalTracer(enabledInternalTracer);
            if (!string.IsNullOrEmpty(trialsFields))
                options.SetFieldTrials(trialsFields);
            PeerConnectionFactory.Initialize(options.CreateInitializationOptions());
            Classes.NativeFactory.Init(new NativeFactory(application));
        }
    }
}
