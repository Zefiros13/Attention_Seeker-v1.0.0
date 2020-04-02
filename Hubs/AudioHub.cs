using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Attention_Seeker.Hubs
{
    public class AudioHub : Hub
    {
        public void Hello()
        {
            Clients.All.playAudio();
        }
    }
}