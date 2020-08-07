// onotseike@hotmail.comPaula Aliu
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebRTC.Signal.Server.Models
{
    public class Room
    {
        #region Properties

        public Guid RoomId { get; set; }
        public List<Client> Occupants { get; set; }
        public bool IsVideo { get; set; }
        private int MaxOccupancy { get; set; }

        #endregion

        public Room(string _roomId, int _maxOccupancy)
        {
            RoomId = new Guid(_roomId);
            MaxOccupancy = _maxOccupancy;
            Occupants = new List<Client>();
        }

        #region Function(s)

        public Tuple<bool, string> AddClient(Client _client)
        {
            if (Occupants?.Count < MaxOccupancy)
            {
                Occupants?.Add(_client);
                return Tuple.Create(true, $"Client with ID : {_client.ClientId} was added to Room with RoomId : {RoomId}");
            }
            if ((bool)Occupants?.Any(occupant => occupant.IsInitiator) && _client.IsInitiator)
            {
                Occupants?.Add(_client);
                return Tuple.Create(true, $"Client with ID : {_client.ClientId} was added to Room with RoomId : {RoomId} as a participant.");
            }

            return Tuple.Create(false, $"The Room with ID : {RoomId} has reached MAXIMUM Occupancy of {MaxOccupancy}");
        }

        public Tuple<bool, string> RemoveClient(Client _client) => (bool)Occupants?.Remove(_client) ? Tuple.Create(true, $"The Client with ID : {_client.ClientId} was added as an occupant of the Room with RoomId : {RoomId}") : Tuple.Create(false, $"The Client with ID : {_client.ClientId} is not an Occupant of the Room with RoomId : {RoomId}");

        public Tuple<bool, string> IsClientAnOccupant(Client _client) => (bool)Occupants?.Any(occupant => occupant.ClientId == _client.ClientId) ? Tuple.Create(true, $"Client with ID : {_client.ClientId} is an Occupant of Room with RoomId : {RoomId}") : Tuple.Create(false, $"The Client with ID : {_client.ClientId} is not an Occupant of the Room with RoomId : {RoomId}");


        #endregion

        #region Exception(s)

        private Exception MaxOccupancyException(string _roomId, string _maxOccupancy) => new Exception($"The Room with ID : {_roomId} has reached MAXIMUM Occupancy of {_maxOccupancy}");

        #endregion
    }
}
