// onotseike@hotmail.comPaula Aliu
namespace WebRTC.Interfaces
{
    public interface IVideoTrack : IMediaStreamTrack
    {
        void AddRenderer(IVideoRenderer videoRenderer);
        void RemoveRenderer(IVideoRenderer videoRenderer);
    }
}