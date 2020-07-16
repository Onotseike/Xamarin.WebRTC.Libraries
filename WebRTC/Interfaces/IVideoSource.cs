// onotseike@hotmail.comPaula Aliu
namespace WebRTC.Interfaces
{
    public interface IVideoSource : IMediaSource
    {
        void AdaptOutputFormat(int width, int height, int fps);
    }
}