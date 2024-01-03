using ApplicationCore.Services;
using DevExtreme.AspNet.Mvc;
using Infraestructure.Models.Catalogo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions)
        {
            IServiceCliente serviceCliente = new ServiceCliente();
            List<Cliente> lista = null;
            try
            {
                loadOptions.SortByPrimaryKey = false;
                lista = serviceCliente.GetCliente().ToList();
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
            IServiceCliente serviceCliente = new ServiceCliente();
            Cliente oCliente = new Cliente();
            try
            {
                JsonConvert.PopulateObject(values, oCliente);

                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No se pudo salvar la información. [ModelState]");
                }

                if (serviceCliente.Save(oCliente) == null)
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
        public ActionResult Put(string key, String values)
        {
            IServiceCliente serviceCliente = new ServiceCliente();
            Cliente oCliente = new Cliente();

            try
            {
                // El Parámetro values solo trae los properties que se alteraron del objeto.

                // Buscar por Cedula
                oCliente = serviceCliente.GetClienteByID(key);
                // Si no existe
                if (oCliente == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, $"No existe el cliente No. {key}");
                }
                else
                {
                    // Si existe poblar oBodega con los values osea los properties que se actualizaron.
                    JsonConvert.PopulateObject(values, oCliente);
                }


                // Validar el model
                if (!TryValidateModel(oCliente))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No se pudo actualizar la información. [ModelState]");

                // Salvar / Actualizar
                if (serviceCliente.Save(oCliente) == null)
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
        public ActionResult Delete(string  key)
        {
            IServiceCliente serviceCliente = new ServiceCliente();
            try
            {
                serviceCliente.DeleteCliente(key);

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