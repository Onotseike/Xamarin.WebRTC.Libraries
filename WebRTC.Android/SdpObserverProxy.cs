// onotseike@hotmail.comPaula Aliu
using Org.Webrtc;
using WebRTC.Android.Extensions;

namespace WebRTC.Android
{
    public class SdpObserverProxy : Java.Lang.Object, ISdpObserver
    {
        private readonly Interfaces.ISdpObserver _observer;

        public SdpObserverProxy(Interfaces.ISdpObserver observer)
        {
            _observer = observer;
        }

        public void OnCreateFailure(string p0)
        {
            _observer?.OnCreateFailure(p0);
        }

        public void OnCreateSuccess(SessionDescription p0)
        {
            _observer?.OnCreateSuccess(p0.ToNet());
        }

        public void OnSetFailure(string p0)
        {
            _observer.OnSetFailure(p0);
        }

        public void OnSetSuccess()
        {
            _observer.OnSetSuccess();
        }
    }
}