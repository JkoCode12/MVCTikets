using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Infraestructure.Models.Catalogo;
using Infraestructure.Models.DataModels;
using System;
using Newtonsoft.Json;
using System.Net.Http.Formatting;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Reflection;
using ApplicationCore.Services;


namespace MVCTickets.Web
{
    public class TipoEventoController : Controller
    {
        // Validar los roles que pueden acceder
        [Authorize(Roles = "Administrador,Procesos,Reportes,Consultas")]
        public ActionResult Index()
        {
            Log.Info($"Visita {this.GetType().Name} - {MethodBase.GetCurrentMethod()}");
            return View();
        }

        // Validar los roles que pueden acceder
        [Authorize(Roles = "Administrador,Procesos,Reportes,Consultas")]
        public ActionResult TipoEventoForm()
        {
            return View();
        }

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions)
        {
            IServiceTipoEvento serviceTipo = new ServiceTipoEvento();
            List<TipoEvento> lista = null;

            try
            {
                loadOptions.SortByPrimaryKey = false;
                lista = serviceTipo.GetTipoEvento().ToList();
                if (lista != null)
                {
                    return Content(JsonConvert.SerializeObject(lista), "application/json");
                }
                else
                {
                    throw new Exception("No hay información");
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);

            }
        }

        


        [HttpPost]
        public ActionResult Post(string values)
        {
            IServiceTipoEvento serviceTipo = new ServiceTipoEvento();
            TipoEvento oTipo = new TipoEvento();
            try
            {
                JsonConvert.PopulateObject(values, oTipo);

                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No se pudo salvar la información. [ModelState]");
                }
                 

                if (serviceTipo.Save(oTipo) == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No se pudo salvar la información.");                     
                }

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {               
                Log.Error(ex, MethodBase.GetCurrentMethod());
               
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPut]
        public ActionResult Put(Int32 key, String values)
        {
            IServiceTipoEvento serviceTipo = new ServiceTipoEvento();
            TipoEvento oTipo = new TipoEvento();

            try
            {
                // El Parámetro values solo trae los properties que se alteraron del objeto.

                // Buscar por Cedula
                oTipo = serviceTipo.GetTipoEventoByID(Convert.ToInt32(key));
                // Si no existe
                if (oTipo == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, $"No existe el tipo de evento No. {key}");
                }
                else {
                    // Si existe poblar oTipo con los values osea los properties que se actualizaron.
                    JsonConvert.PopulateObject(values, oTipo);
                }
                 
               
                // Validar el model
                if (!TryValidateModel(oTipo))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No se pudo actualizar la información. [ModelState]");

                // Salvar / Actualizar
                if (serviceTipo.Save(oTipo) == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No se pudo actualizar la información. ");
                }


                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpDelete]
        public ActionResult Delete(Int32 key)
        {
            IServiceTipoEvento serviceTipo = new ServiceTipoEvento();
            try
            { 
                serviceTipo.DeleteTipoEvento(Convert.ToInt32(key));

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message); 
            }
        }



        /************************************************************************/
        // Metodos utilizados para Mantenimiento Form<>
        [HttpPost]
        public ActionResult AgregarTipoEvento(TipoEvento tipo)
        {

            IServiceTipoEvento serviceTipo = new ServiceTipoEvento();

            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "ERROR");
                }

                if (serviceTipo.Save(tipo) == null)
                {
                    throw new Exception("No se pudo salvar la información");
                }

                return RedirectToAction("TipoEventoForm", "TipoEvento");

            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                TempData["Type"] = "Fail";
                Log.Error(ex, MethodBase.GetCurrentMethod());
                return RedirectToAction("TipoEventoForm", "TipoEvento");
            }
        }
        public ActionResult ActivarTipo(int id)
        {
            IServiceTipoEvento serviceTipo = new ServiceTipoEvento();
            try
            {
                if (!serviceTipo.ActivateDeactivate(id))
                {
                    throw new Exception("No se pudo desactivar el tipo de evento");
                }

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                TempData["Type"] = "Fail";
                Log.Error(ex, MethodBase.GetCurrentMethod());
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
        }

        public ActionResult InactivarTipoEvento(int id)
        {
            IServiceTipoEvento serviceTipo = new ServiceTipoEvento();
            try
            {
                if (!serviceTipo.ActivateDeactivate(id))
                {
                    throw new Exception("No se pudo desactivar el tipo de evento");
                }

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                TempData["Type"] = "Fail";
                Log.Error(ex, MethodBase.GetCurrentMethod());
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
        }

    }
}