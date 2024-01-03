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
    public class TarjetaController : Controller
    {
        // Validar los roles que pueden acceder
        [Authorize(Roles = "Administrador")]
        public ActionResult Index()
        {
            Log.Info($"Visita {this.GetType().Name} - {MethodBase.GetCurrentMethod()}");
            return View();
        }

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions)
        {
            IServiceTarjeta serviceTarjeta = new ServiceTarjeta();
            List<Tarjeta> lista = null;

            try
            {
                loadOptions.SortByPrimaryKey = false;
                lista = serviceTarjeta.GetTarjeta().ToList();
                if (lista != null)
                {
                    return Content(JsonConvert.SerializeObject(DataSourceLoader.Load(lista, loadOptions)), "application/json");
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
            IServiceTarjeta serviceTarjeta = new ServiceTarjeta();
            Tarjeta oTarjeta = new Tarjeta();
            try
            {
                JsonConvert.PopulateObject(values, oTarjeta);

                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No se pudo salvar la información. [ModelState]");
                }

                if (serviceTarjeta.Save(oTarjeta) == null)
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
            IServiceTarjeta serviceTarjeta = new ServiceTarjeta();
            Tarjeta oTarjeta = new Tarjeta();
            try
            {
                // Buscar por Cedula
                oTarjeta = serviceTarjeta.GetTarjetaByID(Convert.ToInt32(key));
                // Si no existe
                if (oTarjeta == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, $"No existe la Tarjeta No. {key}");
                }
                else {
                    // Si existe poblar oTarjeta con los values osea los properties que se actualizaron.
                    JsonConvert.PopulateObject(values, oTarjeta);
                }
                 
               
                // Validar el model
                if (!TryValidateModel(oTarjeta))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No se pudo actualizar la información. [ModelState]");

                // Salvar / Actualizar
                if (serviceTarjeta.Save(oTarjeta) == null)
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
            IServiceTarjeta serviceTarjeta = new ServiceTarjeta();
            try
            { 
                serviceTarjeta.DeleteTarjeta(Convert.ToInt32(key));

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message); 
            }
        }
    }
}