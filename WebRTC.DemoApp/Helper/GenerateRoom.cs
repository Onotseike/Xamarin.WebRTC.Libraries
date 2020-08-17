// onotseike@hotmail.comPaula Aliu
using System;
using System.Text;

namespace WebRTC.DemoApp.Helper
{
    public static class GenerateRoom
    {
        #region Properties & Variables

        private static readonly Random random = new Random();

        #endregion

        #region Method(s)

        public static string GenerateRoomName(int _length = 6)
        {
            var _stringBuilder = new StringBuilder();

            for (int idx = 0; idx < _length; idx++)
            {
                _stringBuilder.Append(random.Next(10).ToString());
            }

            return _stringBuilder.ToString();
        }

        #endregion
    }
}
