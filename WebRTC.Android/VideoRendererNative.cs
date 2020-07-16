// onotseike@hotmail.comPaula Aliu
using System;
using Org.Webrtc;
using WebRTC.Interfaces;

namespace WebRTC.Android
{
    public class VideoRendererProxy : Java.Lang.Object, IVideoSink, IVideoRenderer
    {
        private IVideoSink _renderer;
        public object NativeObject => this;

        public IVideoSink Renderer
        {
            get => _renderer;
            set
            {
                if (_renderer == this)
                    throw new InvalidOperationException("You can set renderer to self");
                _renderer = value;
            }
        }

        public void OnFrame(VideoFrame p0)
        {
            Renderer?.OnFrame(p0);
        }
    }
}
