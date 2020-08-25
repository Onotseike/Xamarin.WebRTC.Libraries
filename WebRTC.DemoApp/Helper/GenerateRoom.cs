// onotseike@hotmail.comPaula Aliu
using System;
using System.Text;

namespace WebRTC.DemoApp.Helper
{
    public static class GenerateRoom
    {


        #region Method(s)

        public static string GenerateRoomName() => Guid.NewGuid().ToString();

        #endregion
    }
}
