// onotseike@hotmail.comPaula Aliu
using WebRTC.DemoApp.iOS.ViewControllers.Base;
using WebRTC.RTC;

namespace WebRTC.DemoApp.iOS.ViewControllers
{
    public class CallController : CallViewControllerBase<RoomConnectionParameters, SignalingParameters, RTCController>
    {
        #region Properties & Variables

        private readonly string roomId;
        private readonly bool isLoopBack;

        #endregion

        #region Constructor(s)

        public CallController(string _roomId, bool _loopBack = false)
        {
            roomId = _roomId;
            isLoopBack = _loopBack;
        }


        #endregion


        #region Implementations of CallViewControllerBase abstract Methods

        protected override void Connect(RTCController _controller)
        {
            _controller.Connect(new RoomConnectionParameters
            {
                RoomId = roomId,
                IsLoopback = isLoopBack,
                RoomUrl = "https://appr.tc/"
            });
        }

        protected override RTCController CreateController() => new RTCController(this);


        #endregion

    }

}
