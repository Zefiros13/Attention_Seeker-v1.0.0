using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Attention_Seeker.Models
{
    public class Chat : Hub
    {
        public void LetsChat(string name, string message)
        {
            Clients.All.NewMessage(name, message);
        }
    }
}