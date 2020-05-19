using Attention_Seeker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Attention_Seeker.ViewModels
{
    public class UsersMessagesViewModel
    {
        public List<ApplicationUser> Users { get; set; }

        public List<Message> Messages { get; set; }
    }
}