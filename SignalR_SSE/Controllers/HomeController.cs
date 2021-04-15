using SignalR_SSE.SSE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SignalR_SSE.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            
            return View();
        }

        public ActionResult Chat(int id)
        {
            //ConnectionTracker.Instance.AddConnection(id);
            return View(id);
        }
        public ActionResult Hit(int? id)
        {
            NotificationService.Instance.Notify($"Someone hit home: {id}", id);
            return View(ConnectionTracker.Instance.ConnectionCount);
        }
       
    }
}
