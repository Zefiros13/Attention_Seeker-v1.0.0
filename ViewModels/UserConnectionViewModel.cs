using Attention_Seeker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Attention_Seeker.ViewModels
{
    public class UserConnectionViewModel
    {
        public ApplicationUser User { get; set; }

        public UsersConnection Connection { get; set; }
    }
}