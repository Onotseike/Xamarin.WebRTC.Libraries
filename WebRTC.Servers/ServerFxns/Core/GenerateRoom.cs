// onotseike@hotmail.comPaula Aliu
using System;
using System.Text;

namespace WebRTC.Servers.ServerFxns.Core
{
    public static class GenerateRoom
    {
        private static readonly Random Random = new Random();

        public static string GenerateRoomName(int length = 6)
        {
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(Random.Next(10).ToString());
            }
            return stringBuilder.ToString();
        }
    }
}
