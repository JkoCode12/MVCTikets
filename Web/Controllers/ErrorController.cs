using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MVCTickets.Web
{
    public class ErrorController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult Default()
        { 
            
            TempData["Message"] = "Se producto un error, favor notificarlo a T.I. <br>";
            
            return View();
        }
    }
}