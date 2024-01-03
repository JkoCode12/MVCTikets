using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Infraestructure.Models.Catalogo;
using Infraestructure.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MVCTickets.Web
{
    

    public class HomeController : Controller
    {
        
        [Authorize(Roles = "Administrador,Procesos,Reportes,Consultas")]
        
        public ActionResult Index()
        {
           
            return View(SampleData.images);

        }
       
        [Authorize(Roles = "Administrador")]
        public ActionResult AcercaDe()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult CldrData()
        {
            return new DevExtreme.AspNet.Mvc.CldrDataScriptBuilder()
                .SetCldrPath("~/Content/cldr-data")
                .SetInitialLocale("es")
                .UseLocales("es", "es")
                .Build();
        }

        public ActionResult Error(string id)
        {

            Response.StatusCode = int.Parse(id);
            return View();
        }

        public ActionResult ErrorForzado()
        {
            int x = 1;

            if (x == 1)
            {
                throw new Exception("Mi error forzado");
            }
            return View();
        }

        [Authorize(Roles = "Administrador,Procesos,Reportes,Consultas")]
        //Pantalla del Contact
        public ActionResult Contacto()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        [Authorize(Roles = "Administrador,Procesos,Reportes,Consultas")]
        public ActionResult Ayuda()
        {
            ViewBag.Message = "Documentos Importantes";
            return View();
        }

    }
}