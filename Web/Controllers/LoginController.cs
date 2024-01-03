using ApplicationCore.Services;
using Infraestructure.Models.Catalogo;
using Infraestructure.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVCTickets.Web
{
    public class LoginController : Controller
    {

        public LoginController()
        {
        }


        public ActionResult Index(string message = "")
        {
            ViewBag.Message = null;
            ViewBag.Codigo = null;
            ViewBag.Contrasena = null;

            if (message.Contains("El usuario es requerido"))
            {
                ViewBag.Codigo = message;
            }
            else if (message.Contains("La contraseña es requerida"))
            {
                ViewBag.Contrasena = message;
            }
            ViewBag.Message = message;

            return View();
        }

        [HttpPost]
        public ActionResult Login(string codigo, string contrasena)
        {
            IServiceEmpleado serviceUsuario = new ServiceEmpleado();
            
            try
            {
                Log.Info($"Login {codigo}");

                //DataModel db = new DataModel();
                if (!string.IsNullOrEmpty(codigo) && !string.IsNullOrEmpty(contrasena))
                {
                    // var user = db.Empleado.FirstOrDefault(e => e.Login == codigo.Trim().ToString() && e.Password == contrasena.Trim().ToString());
                    Empleado usuario = serviceUsuario.GetEmpleado(codigo, contrasena);
                    if (usuario != null)
                    {
                        FormsAuthentication.SetAuthCookie(usuario.Login, true);
                        return RedirectToAction("Index", "Home");

                    }
                    else
                    {
                        Log.Error($"Login {codigo} : No encontramos tus datos");
                        return RedirectToAction("Index", "Login", new { message = "Los datos no corresponden con un usuario registrado" });
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(codigo))
                    {
                        return RedirectToAction("Index", "Login", new { message = "El usuario es requerido" });
                    }
                    else if (string.IsNullOrEmpty(contrasena))
                    {
                        return RedirectToAction("Index", "Login", new { message = "La contraseña es requerida" });
                    }
                    else {
                        Log.Error($"Login {codigo} :Los valores de usuario y contraseña deben estar llenos");
                        return RedirectToAction("Index", "Login", new { message = "Los valores de usuario y contraseña deben estar llenos" });
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = ex.Message;
                TempData["Type"] = "Fail";
                return View("Index");
            }
        }

        [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }
}
