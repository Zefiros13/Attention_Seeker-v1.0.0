using Attention_Seeker.Models;
using Attention_Seeker.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

namespace Attention_Seeker.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        public ApplicationDbContext _context{ get; set; }

        public UsersController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Index()
        {
            var users = _context.Users.ToList();

            return View(users);
        }

        [Route("Users/Edit/{id}")]
        public ActionResult EditProfile(string id)
        {
            var currentUrlId = Url.RequestContext.RouteData.Values["id"].ToString();
            if (currentUrlId != System.Web.HttpContext.Current.User.Identity.GetUserId())
                return RedirectToAction("Index", "Users");

            var user = _context.Users.SingleOrDefault(u => u.Id == id);
            
            return View(user);
        }

        [Route("Users/Profile/{id}")]
        public ActionResult ReadOnlyProfile(string id, string conne)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == id);

            var logedInUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentUrlId = Url.RequestContext.RouteData.Values["id"].ToString();

            var connInDb = _context.Connections.FirstOrDefault(c => (c.ConnectionReceiver.Id == currentUrlId && c.ConnectionSender.Id == logedInUserId) 
                                                                 || (c.ConnectionReceiver.Id == logedInUserId && c.ConnectionSender.Id == currentUrlId));
            if (connInDb == null)
            {
                var newConnection = new UsersConnection()
                {
                    ConnectionReceiver = user,
                    ConnectionSender = _context.Users.SingleOrDefault(u => u.Id == logedInUserId),
                    WaitingFlag = false,
                    ApproveFlag = false,
                    RejectFlag = false,
                    BlockFlag = false,
                    SpamFlag = false,
                    DateCreated = DateTime.Now
                };

                _context.Connections.Add(newConnection);
                _context.SaveChanges();

                //var timeLimit = new TimeSpan(48, 0, 0);
                //if (newConnection.WaitingFlag == false && newConnection.ApproveFlag == false && newConnection.DateCreated + timeLimit > DateTime.Now)
                //{
                //    _context.Connections.Remove(newConnection);
                //    _context.SaveChanges();
                //}

                var viewModel = new UserConnectionViewModel()
                {
                    User = user,
                    Connection = newConnection
                };

                return View(viewModel);
            }
            else
            {
                var viewModel = new UserConnectionViewModel()
                {
                    User = user,
                    Connection = connInDb
                };
                switch (conne)
                {
                    case "ask":
                        {
                            connInDb.WaitingFlag = true;
                            _context.SaveChanges();
                            return View(viewModel);
                        }

                    case "cancel":
                        {
                            connInDb.WaitingFlag = false;
                            _context.SaveChanges();
                            return View(viewModel);
                        }
                    case "disconnect":
                        {
                            connInDb.WaitingFlag = false;
                            connInDb.ApproveFlag = false;
                            _context.Connections.Remove(connInDb);
                            _context.SaveChanges();
                            return View(viewModel);
                        }
                    default:
                        return View(viewModel);

                }

            }



            

        }



        [HttpPost]
        public ActionResult Save(ApplicationUser user)
        {
            if (!ModelState.IsValid)
                return View("EditProfile", user);
            
            var userInDb = _context.Users.Single(u => u.Id == user.Id);

            userInDb.Name = user.Name;
            userInDb.Bio = user.Bio;
            userInDb.UserName = user.UserName;

            _context.SaveChanges();

            return RedirectToAction("Index", "Users");

        }

        [ChildActionOnly]
        public ActionResult _Connections()
        {
            var logedInUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            var connInDb = _context.Connections.Where(c => c.ConnectionReceiver.Id == logedInUserId && c.WaitingFlag == true && c.ApproveFlag == false).ToList();

            var viewModel = new UsersConnectionsViewModel
            {
                Users = _context.Users.ToList(),
                Connections = connInDb
            };

            return PartialView(viewModel);
        }

        [ChildActionOnly]
        public ActionResult _CurrentlyPaired()
        {
            var logedInUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            var connections = _context.Connections.Where(c => (c.ConnectionReceiver.Id == logedInUserId || c.ConnectionSender.Id == logedInUserId) && c.ApproveFlag == true).ToList();

            var model = new UsersConnectionsViewModel
            {
                Users = _context.Users.ToList(),
                Connections = connections
            };

            return PartialView(model);
        }
        
        public ActionResult Approve(int Id)
        {
            var connection = _context.Connections.SingleOrDefault(c => c.Id == Id);

            connection.ApproveFlag = true;
            connection.WaitingFlag = false;
            _context.SaveChanges();

            return RedirectToAction("Index", "Users");
        }

        public ActionResult Reject(int Id)
        {
            var connection = _context.Connections.SingleOrDefault(c => c.Id == Id);

            connection.WaitingFlag = false;
            _context.Connections.Remove(connection);
            _context.SaveChanges();

            return RedirectToAction("Index", "Users");
        }




    }
}