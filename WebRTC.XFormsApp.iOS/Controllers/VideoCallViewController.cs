// onotseike@hotmail.comPaula Aliu
using System;

using WebRTC.Servers.ServerFxns.Core;
using WebRTC.XFormsApp.iOS.ViewControllerBases;

namespace WebRTC.XFormsApp.iOS.Controllers
{
    public class VideoCallViewController : CallViewControllerBase<RoomConnectionParameters, SignalingParameters, RTCController>
    {
        #region Properties & Variables

        private readonly string roomId;
        private readonly bool isLoopBack;

        #endregion

        #region Constructor(s)

        public VideoCallViewController(string _roomId, bool _loopBack = false)
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
                RoomID = roomId,
                IsLoopBack = isLoopBack,
                RoomURL = "https://appr.tc"
            });
        }

        protected override RTCController CreateController() => new RTCController(this);


        #endregion

    }
}
