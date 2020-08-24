using System.Collections.Generic;

namespace WebRTC.Signal.Server.Models
{
	public  static class HubObjects
	{
		public static List<Client> Clients { get; set; } = new List<Client>();
		public static  List<Room> Rooms { get; set; } = new List<Room>();
		public static  List<CallOffer> CallOffers { get; set; } = new List<CallOffer>();
	}
}