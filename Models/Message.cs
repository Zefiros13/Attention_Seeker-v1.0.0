using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Attention_Seeker.Models
{
    public class Message
    {
        public int Id { get; set; }

        public UsersConnection Connection { get; set; }

        public ApplicationUser MessageSender { get; set; }

        public ApplicationUser MessageReceiver { get; set; }

        public string MessageContent { get; set; }

        public bool IsSeen { get; set; }
    }
}