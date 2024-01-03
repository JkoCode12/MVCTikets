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
using System.IO;


namespace MVCTickets.Web
{
    public class EmpleadoController : Controller
    {
        private DataModel db;

        public EmpleadoController()
        {
            this.db = new DataModel();
        }

        [Authorize(Roles = "Administrador")]
        //Pantalla de Empleado
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions)
        {
            try
            {
                loadOptions.SortByPrimaryKey = false;
                List<Empleado> list = db.Empleado.ToList();
                var lista = DataSourceLoader.Load(list, loadOptions);
                return Content(JsonConvert.SerializeObject(lista), "application/json");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message.ToString());
            }
        }


        [HttpPost]
        public ActionResult Post(string values)
        {
            try
            {
                //var values = form.Get("values");
                var model = new Empleado();
                JsonConvert.PopulateObject(values, model);

                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "ERROR");
                }

                db.Empleado.Add(model);
                db.SaveChanges();

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPut]
        public ActionResult Put(Int32 key, String values)
        {
            try
            {

                //var key = form.Get("key");
                //var values = form.Get("values");
                var model = db.Empleado.First(e => e.Cedula == key);

                JsonConvert.PopulateObject(values, model);

                if (!TryValidateModel(model))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Error");

                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();


                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpDelete]
        public ActionResult Delete(Int32 key)
        {

            try
            {
                //var key = form.Get("key");
                var model = db.Empleado.First(a => a.Cedula == key);
                db.Empleado.Remove(model);
                db.SaveChanges();

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        /*
        public ActionResult Upload()
        {
            try
            {
                //IFormFile file = Request.Form.Files[0];
                //using (Stream stream = file.OpenReadStream())
                //{
                //    HttpContext.Session.SetString("Foto", ConvertToBase64(stream));
                //}
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
            }
            return new EmptyResult();
        }
        */
        public string ConvertToBase64(Stream stream)
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            string base64 = Convert.ToBase64String(bytes);
            return base64;
            //  return new MemoryStream(Encoding.UTF8.GetBytes(base64));
        }

    }
}