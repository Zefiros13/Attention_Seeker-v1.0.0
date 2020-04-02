using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Attention_Seeker.Models;
using Attention_Seeker.ViewModels;
using System.Web.Mvc;


namespace Attention_Seeker.Models
{
    public class UsersConnection
    {
        public int Id { get; set; }

        [Required]
        public ApplicationUser ConnectionSender { get; set; }

        [Required]
        public ApplicationUser ConnectionReceiver { get; set; }

        public bool WaitingFlag { get; set; }

        public bool ApproveFlag { get; set; }

        public bool RejectFlag { get; set; }

        public bool BlockFlag { get; set; }

        public bool SpamFlag { get; set; }

        public DateTime DateCreated { get; set; }
    }
}