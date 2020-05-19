using Attention_Seeker.Models;
using Attention_Seeker.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
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
        public ActionResult ReadOnlyProfile(string id, string buttonClick)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == id);

            var logedInUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var currentUrlId = Url.RequestContext.RouteData.Values["id"].ToString();

            var connectionInDb = _context.Connections.FirstOrDefault(c => (c.ConnectionReceiver.Id == currentUrlId && c.ConnectionSender.Id == logedInUserId) 
                                                                       || (c.ConnectionReceiver.Id == logedInUserId && c.ConnectionSender.Id == currentUrlId));
            if (connectionInDb == null)
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
                    DateCreated = DateTime.Now,
                    Messages = new List<Message>()
                };

                _context.Connections.Add(newConnection);
                _context.SaveChanges();

                //never tested, should delete unused connection after 2 days-
                //var timeLimit = new TimeSpan(48, 0, 0);
                //if (newConnection.WaitingFlag == false && newConnection.ApproveFlag == false && newConnection.DateCreated + timeLimit > DateTime.Now)
                //{
                //    _context.Connections.Remove(newConnection);
                //    _context.SaveChanges();
                //}

                var viewModel = new UserConnectionViewModel()
                {
                    User = user,
                    Connection = newConnection,
                    Messages = newConnection.Messages
                };

                return View(viewModel);
            }
            else
            {
                var viewModel = new UserConnectionViewModel()
                {
                    User = user,
                    Connection = connectionInDb,
                    Messages = _context.Messages.Where(c => c.Connection.Id == connectionInDb.Id).ToList()
                };

                switch (buttonClick)
                {
                    case "sendRequest":
                        {
                            connectionInDb.WaitingFlag = true;
                            _context.SaveChanges();
                            return View(viewModel);
                        }

                    case "cancelRequest":
                        {
                            connectionInDb.WaitingFlag = false;
                            _context.SaveChanges();
                            return View(viewModel);
                        }
                    case "cancelConnection":
                        {
                            connectionInDb.WaitingFlag = false;
                            connectionInDb.ApproveFlag = false;
                            _context.Connections.Remove(connectionInDb);
                            _context.SaveChanges();
                            return View(viewModel);
                        }
                    default:
                        return View(viewModel);

                }
            }
        }

        [HttpPost]
        public ActionResult Save(ApplicationUser user, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
                return View("EditProfile", user);
            
            var userInDb = _context.Users.Single(u => u.Id == user.Id);

            userInDb.Name = user.Name;
            userInDb.Bio = user.Bio;
            userInDb.UserName = user.UserName;

            if (file != null && file.ContentLength > 0)
            {
                var path = Path.Combine(Server.MapPath("~/Media/ProfilePictures"), Path.GetFileName(file.FileName));
                file.SaveAs(path);
                userInDb.ProfilePicturePath = "/Media/ProfilePictures/" + Path.GetFileName(file.FileName).ToString();
            }
            else
                userInDb.ProfilePicturePath = userInDb.ProfilePicturePath;

            _context.SaveChanges();

            return RedirectToAction("Index", "Users");
        }

        [ChildActionOnly]
        public ActionResult _Connections()
        {
            var logedInUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            var connectionsInDb = _context.Connections.Where(c => c.ConnectionReceiver.Id == logedInUserId && c.WaitingFlag == true && c.ApproveFlag == false).ToList();

            var viewModel = new UsersConnectionsViewModel
            {
                Users = _context.Users.ToList(),
                Connections = connectionsInDb
            };

            return PartialView(viewModel);
        }

        [ChildActionOnly]
        public ActionResult _CurrentlyPaired()
        {
            var logedInUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            var currentlyPairedConnections = _context.Connections.Where(c => (c.ConnectionReceiver.Id == logedInUserId || c.ConnectionSender.Id == logedInUserId) && c.ApproveFlag == true).ToList();

            var model = new UsersConnectionsViewModel
            {
                Users = _context.Users.ToList(),
                Connections = currentlyPairedConnections
            };

            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult _NewMessages()
        {
            var logedInUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var connectionsInDb = _context.Connections.Where(c => (c.ConnectionReceiver.Id == logedInUserId && c.WaitingFlag == false && c.ApproveFlag == true) 
                                                                          ||(c.ConnectionSender.Id == logedInUserId && c.WaitingFlag == false && c.ApproveFlag == true)).ToList();
            var messages = _context.Messages.Where(m => m.MessageReceiver.Id == logedInUserId).Where(m => m.IsSeen == false && m.Connection != null).ToList();

            var viewModel = new UsersMessagesViewModel
            {
                Users = _context.Users.ToList(),
                Messages = messages
            };

            return PartialView(viewModel);
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

        public ActionResult SeeNewMessage(string Id)
        {
            var currentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var messages = _context.Messages.Where(m => (m.MessageSender.Id == Id && m.MessageReceiver.Id == currentUserId) 
                                                      ||(m.MessageSender.Id == currentUserId && m.MessageReceiver.Id == Id));

            foreach(var message in messages)
            {
                message.IsSeen = true;
            }
            
            _context.SaveChanges();

            return RedirectToAction("ReadOnlyProfile", "Users");
        }

        [HttpPost]
        [Route("Chat/{id}")]
        public ActionResult SendMessage(string id, string message)
        {
            var currentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            var connection = _context.Connections.SingleOrDefault(c => (c.ConnectionReceiver.Id == currentUserId && c.ConnectionSender.Id == id)
                                                                    || (c.ConnectionReceiver.Id == id && c.ConnectionSender.Id == currentUserId));

            var newMessage = new Message
            {
                Connection = connection,
                MessageSender = _context.Users.SingleOrDefault(u => u.Id == currentUserId),
                MessageReceiver = _context.Users.SingleOrDefault(u => u.Id == id),
                MessageContent = message,
                IsSeen = false
            };

            _context.Messages.Add(newMessage);
            _context.SaveChanges();

            return RedirectToAction("ReadOnlyProfile", "Users");
        }

    }
}