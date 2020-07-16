// onotseike@hotmail.comPaula Aliu
using System;
using Org.Webrtc;
using WebRTC.Classes;
using WebRTC.Extensions;
using WebRTC.Interfaces;

namespace WebRTC.Android
{
    internal class MediaStreamNative : NativeObjectBase, IMediaStream
    {
        private readonly MediaStream _mediaStream;

        public MediaStreamNative(MediaStream mediaStream) : base(mediaStream)
        {
            _mediaStream = mediaStream;
        }


        public string StreamId => _mediaStream.Id;

        public IAudioTrack[] AudioTracks => GetAudioTracks();

        public IVideoTrack[] VideoTracks => GetVideoTracks();

        public void AddTrack(IAudioTrack audioTrack)
        {
            _mediaStream.AddTrack(audioTrack.ToNative<AudioTrack>());
        }

        public void AddTrack(IVideoTrack videoTrack)
        {
            _mediaStream.AddTrack(videoTrack.ToNative<VideoTrack>());
        }

        public void RemoveTrack(IAudioTrack audioTrack)
        {
            _mediaStream.RemoveTrack(audioTrack.ToNative<AudioTrack>());
        }

        public void RemoveTrack(IVideoTrack videoTrack)
        {
            _mediaStream.RemoveTrack(videoTrack.ToNative<VideoTrack>());
        }

        private IAudioTrack[] GetAudioTracks()
        {
            var items = _mediaStream.AudioTracks;
            var arr = new IAudioTrack[items.Count];
            for (int i = 0; i < items.Count; i++)
            {
                arr[i] = new AudioTrackNative((AudioTrack)items[i]);
            }

            return arr;
        }

        private IVideoTrack[] GetVideoTracks()
        {
            var items = _mediaStream.VideoTracks;
            var arr = new IVideoTrack[items.Count];
            for (int i = 0; i < items.Count; i++)
            {
                arr[i] = new VideoTrackNative((VideoTrack)items[i]);
            }

            return arr;
        }
    }
}
