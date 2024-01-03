using ApplicationCore.Services;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Infraestructure.Models.Catalogo;
using Infraestructure.Models.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class ProductoController : Controller
    {
        // GET: Producto
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions)
        {
            IServiceProducto serviceProducto = new ServiceProducto();
            List<Producto> lista = null;
            try
            {
                loadOptions.SortByPrimaryKey = false;
                lista = serviceProducto.GetProducto().ToList();
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

        [HttpGet]
        public object GetProductoByID(int id)
        {
            IServiceProducto serviceProducto = new ServiceProducto();
             Producto  Producto = null;
            try
            {
                
                Producto = serviceProducto.GetProductoByID(id);
                if (Producto != null)
                {
                    return Content(JsonConvert.SerializeObject(Producto), "application/json");
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
            IServiceProducto serviceProducto = new ServiceProducto();
            IServiceTipoEvento serviceTipoEvento = new ServiceTipoEvento();
            Producto oProducto = new Producto();
            try
            {
                JsonConvert.PopulateObject(values, oProducto);

                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No se pudo salvar la información. [ModelState]");
                }
                if (serviceProducto.GetProductoByID(oProducto.IdProducto) != null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No se pudo salvar la información. El codigo de evento ya existe.");
                }
                if (serviceProducto.Save(oProducto) == null )
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
            IServiceProducto serviceProducto = new ServiceProducto();
            Producto oProducto = new Producto();

            try
            {
                // El Parámetro values solo trae los properties que se alteraron del objeto.

                // Buscar por Cedula
                oProducto = serviceProducto.GetProductoByID(Convert.ToInt32(key));
                // Si no existe
                if (oProducto == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, $"No existe el evento No. {key}");
                }
                else
                {
                    // Si existe poblar oProducto con los values osea los properties que se actualizaron.
                    JsonConvert.PopulateObject(values, oProducto);
                }


                // Validar el model
                if (!TryValidateModel(oProducto))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No se pudo actualizar la información. [ModelState]");

                // Salvar / Actualizar
                if (serviceProducto.Save(oProducto) == null)
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
            IServiceProducto serviceProducto = new ServiceProducto();
            try
            {
                serviceProducto.DeleteProducto(Convert.ToInt32(key));

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