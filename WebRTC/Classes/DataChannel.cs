// onotseike@hotmail.comPaula Aliu
using System;
using WebRTC.Enums;

namespace WebRTC.Classes
{
    public class DataChannel
    {
        public class DataBuffer
        {
            public DataBuffer(byte[] data, bool isBinary)
            {
                Data = data;
                IsBinary = isBinary;
            }

            public bool IsBinary { get; }
            public byte[] Data { get; }
        }

        public interface IDataChannel : IDisposable
        {
            event EventHandler OnStateChange;
            event EventHandler<DataBuffer> OnMessage;
            event EventHandler<long> OnBufferedAmountChange;

            int Id { get; }
            string Label { get; }
            DataChannelState State { get; }
            long BufferedAmount { get; }

            void Close();
            bool Send(DataBuffer dataBuffer);
        }

        public class DataChannelConfiguration
        {
            public int Id { get; set; } = -1;
            public int MaxRetransmitTimeMs { get; set; } = -1;
            public int MaxRetransmits { get; set; } = -1;
            public bool IsOrdered { get; set; } = true;
            public string Protocol { get; set; } = "";
            public bool IsNegotiated { get; set; }
        }
    }
}
