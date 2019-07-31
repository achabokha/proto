using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;

namespace EmbilyServices.Hubs
{
    public class BlockchainHub : Hub
    {
        private readonly IHostingEnvironment _env;

        public BlockchainHub(IHostingEnvironment env)
        {
            _env = env;
        }

        public async Task Echo(string message)
        {
            await Clients.All.SendAsync("Echo", message);
        }
    }
}
