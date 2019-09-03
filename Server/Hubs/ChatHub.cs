using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;

namespace Server.Hubs
{
	public class ChatHub: Hub
	{
		public async Task SendToAll(string name, string message)
		{
			await Clients.All.SendAsync("sendToAll", name, message);
		}
	}
}