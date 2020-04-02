using Attention_Seeker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Attention_Seeker.ViewModels
{
    public class UsersConnectionsViewModel
    {
        public List<ApplicationUser> Users{ get; set; }

        public List<UsersConnection> Connections { get; set; }
    }
}