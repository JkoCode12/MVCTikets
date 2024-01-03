using ApplicationCore.Services;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Infraestructure.Models.Catalogo;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

namespace Web.Controllers
{
    public class FacturaController : Controller
    {
        // Validar los roles que pueden acceder


        [Authorize(Roles = "Administrador,Procesos,Reportes,Consultas")]
        public ActionResult Index()
        {
            Log.Info($"Visita {this.GetType().Name} - {MethodBase.GetCurrentMethod()}");
            return View();
        }

        public ActionResult FacturaConsulta()
        {
            Log.Info($"Visita {this.GetType().Name} - {MethodBase.GetCurrentMethod()}");
            return View("FacturaConsulta");
        }


        //*****************************************************************//
        //
        //                         Encabezado de la FACTURA 
        //
        //*****************************************************************//



        //Metodos a utilizar
        [HttpGet]
        //Muestra el contenido General de la base de datos se traerá TODOS los estados
        public object GetFacturaEncabezado(DataSourceLoadOptions loadOptions)
        {
            IServiceFactura serviceFactura = new ServiceFactura();
            List<FacturaEncabezado> lista = null;

            try
            {
                loadOptions.SortByPrimaryKey = false;
                lista = serviceFactura.GetFacturaEncabezado().ToList();
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
        //Muestra el contenido General de la base de datos se traerá UNICAMENTE dónde el ESTADO es PENDIENTE
        public object GetFacturaPendientes(DataSourceLoadOptions loadOptions)
        {
            IServiceFactura serviceFactura = new ServiceFactura();
            List<FacturaEncabezado> lista = null;

            try
            {
                loadOptions.SortByPrimaryKey = false;
                lista = serviceFactura.GetFacturaEncabezadoPendientes().ToList();
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
        //Muestra el contenido General de la base de datos se traerá UNICAMENTE los datos que CUMPLAN CON LOS FILTROS REALIZADOS
        public object GetFacturasByDate(DataSourceLoadOptions loadOptions)
        {
            IServiceFactura serviceFactura = new ServiceFactura();
            List<FacturaEncabezado> lista = null;

            try
            {
                //Limpiamos la opcion de que se ordene por llave primaria
                loadOptions.SortByPrimaryKey = false;
                //Consultamos si posee filtros activos
                if (loadOptions.Filter != null)
                {
                    //Dado que la fecha inicial y fecha final son requeridos podemos estar seguros que estos filtros SIEMPRE van a estar
                    //Obtenemos la fecha Inicial del JARRAY en la posición 0 se le concatena el time 00:00:00 para abarcar desde el inicio de la consulta
                    DateTime fechaInicial = DateTime.ParseExact(((JArray)loadOptions.Filter[0])[1].ToString() + " 00:00:00", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    //Obtenemos la fecha Final del JARRAY en la posición 1 se le concatena el time 23:59:59 para abarcar desde el inicio de la consulta
                    DateTime fechaFinal = DateTime.ParseExact(((JArray)loadOptions.Filter[1])[1].ToString() + " 23:59:59", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    //Obtenemos el estado de la consulta en la posición 2 si no posee datos se le asignará TODO
                    //Dado que el enum asignado para este punto tanto el value como display es el texto se tomará como string
                    String estadoFac = ((JArray)loadOptions.Filter[2])[1].ToString() == null ? "TODO" : (((JArray)loadOptions.Filter[2])[1].ToString());
                    //Se crea una variable de tipo Enum TypeEstadoTarjetaConsulta
                    TypeEstadoFacturaConsulta estado;
                    //Se crea variable entera para guardar el estado
                    int pEstadoFactura;
                    //En caso de que sea TODO se asignara el valor cero
                    if (estadoFac == "TODO")
                    {
                        pEstadoFactura = 0;
                    }
                    else
                    {
                        //En caso de que el resultado sea diferente a TODO se obtiene el valor del enum que equivale al contenido del string 
                        estado = (TypeEstadoFacturaConsulta)Enum.Parse(typeof(TypeEstadoFacturaConsulta), estadoFac, true);
                        //Se obtiene el valor que corresponde a la variable del enum
                        pEstadoFactura = (int)estado;
                    }
                    //Se limpian los filtros
                    loadOptions.Filter = null;
                    //Se hace la invocacion de la consulta al método correspondiente enviando por parametros los valores requeridos
                    lista = serviceFactura.GetFacturaEncabezadoConsulta(fechaInicial, fechaFinal, pEstadoFactura).ToList();
                }
                if (lista != null)
                {
                    //Retorna la lista con los valores correspondientes
                    return Content(JsonConvert.SerializeObject(DataSourceLoader.Load(lista, loadOptions)), "application/json");
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);

            }
        }

        [HttpPost]
        public ActionResult PostFacturaEncabezado(string values)
        {
            IServiceFactura serviceFactura = new ServiceFactura();
            FacturaEncabezado oFacturaEncabezado = new FacturaEncabezado();
            try
            {
                JsonConvert.PopulateObject(values, oFacturaEncabezado);

                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No se pudo salvar la información. [ModelState]");
                }

                if (serviceFactura.SaveFacturaEncabezado(oFacturaEncabezado) == null)
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
        public ActionResult PutFacturaEncabezado(Int32 key, String values)
        {
            IServiceFactura serviceFactura = new ServiceFactura();
            FacturaEncabezado oFacturaEncabezado = new FacturaEncabezado();

            try
            {
                // El Parámetro values solo trae los properties que se alteraron del objeto.

                // Buscar por Cedula
                oFacturaEncabezado = serviceFactura.GetFacturaEncabezadoById(Convert.ToInt32(key));
                // Si no existe
                if (oFacturaEncabezado == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, $"No existe la FacturaEncabezado No. {key}");
                }
                else
                {
                    // Si existe poblar oFacturaEncabezado con los values osea los properties que se actualizaron.
                    JsonConvert.PopulateObject(values, oFacturaEncabezado);
                }


                // Validar el model
                if (!TryValidateModel(oFacturaEncabezado))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No se pudo actualizar la información. [ModelState]");

                // Salvar / Actualizar
                if (serviceFactura.SaveFacturaEncabezado(oFacturaEncabezado) == null)
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
        public ActionResult DeleteFacturaEncabezado(Int32 key)
        {
            IServiceFactura serviceFactura = new ServiceFactura();
            try
            {
                serviceFactura.DeleteFacturaEncabezado(Convert.ToInt32(key));

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        public ActionResult Facturar(int id)
        {
            IServiceFactura serviceFactura = new ServiceFactura();

            try
            {
                int valor = serviceFactura.GetFacturaDetalle(id).Count();
                if (valor <= 0)
                {
                    Response.StatusCode = 400;
                    return Content("No se puede generar la factura si no posee un detalle");
                }

                if (!serviceFactura.Facturar(id))
                {
                    throw new Exception("No se pudo Procesar la Factura");
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

        [HttpGet]
        public ActionResult GetFacturaEncabezadoById(int id)
        {
            IServiceFactura serviceFactura = new ServiceFactura();
            FacturaEncabezado lista = null;
            try
            {
                lista = serviceFactura.GetFacturaEncabezadoById(id);
                if (lista != null)
                {
                    return Content(JsonConvert.SerializeObject(lista.EstadoFactura), "application/json");
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
        public ActionResult SendFacturaEmail(int id)
        {
            IServiceFactura serviceFacturaEncabezado = new ServiceFactura();
            IServiceCliente serviceCliente = new ServiceCliente();
            try
            {
                String CuentaCorreoElectronico = "tickethostatencionalcliente@gmail.com";
                String ContrasenaGeneradaGmail = "hnbazjcmrkpghbkx";
                MailMessage mensaje = new MailMessage();
                mensaje.IsBodyHtml = true;
                mensaje.Subject = "TIKETHOST - Factura Eletronica - " + id.ToString();
                mensaje.Body = "Hola," + serviceCliente.GetClienteByID(serviceFacturaEncabezado.GetFacturaEncabezadoById(id).IdCliente).MostrarCliente + " te adjunto tu factura electronica y tus boletos. Muchas gracias por usar los servicios de TiketHost.";
                mensaje.From = new MailAddress(CuentaCorreoElectronico);
                mensaje.To.Add(serviceCliente.GetClienteByID(serviceFacturaEncabezado.GetFacturaEncabezadoById(id).IdCliente).Email);  //Correo del destinatario
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Port = 587;
                smtp.Credentials = new NetworkCredential(CuentaCorreoElectronico, ContrasenaGeneradaGmail);
                smtp.EnableSsl = true;
                Attachment attachment = new Attachment(@"C:\temp\factura" + id + ".pdf");
                mensaje.Attachments.Add(attachment);

                smtp.Send(mensaje);

                SendBoletoEmail(id);

                return new HttpStatusCodeResult(HttpStatusCode.OK, "Correo enviado correctamente");
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        public ActionResult SendBoletoEmail(int IdFactura)
        {
            IServiceFactura serviceFactura = new ServiceFactura();
            IServiceCliente serviceCliente = new ServiceCliente();
            FacturaDetalle oDetalle = new FacturaDetalle();
            List<FacturaDetalle> listaDetalle = serviceFactura.GetFacturaDetalle(IdFactura).ToList();
            int index = 0;

            try
            {
                String CuentaCorreoElectronico = "tickethostatencionalcliente@gmail.com";
                String ContrasenaGeneradaGmail = "hnbazjcmrkpghbkx";
                MailMessage mensaje = new MailMessage();
                mensaje.IsBodyHtml = true;
                mensaje.Subject = "TIKETHOST - BoletoElectronico";
                mensaje.Body = "Hola," + serviceCliente.GetClienteByID(serviceFactura.GetFacturaEncabezadoById(IdFactura).IdCliente).MostrarCliente + " te adjunto tus boletos. Muchas gracias por usar los servicios de TiketHost.";
                mensaje.From = new MailAddress(CuentaCorreoElectronico);
                mensaje.To.Add(serviceCliente.GetClienteByID(serviceFactura.GetFacturaEncabezadoById(IdFactura).IdCliente).Email);  //Correo del destinatario
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Port = 587;
                smtp.Credentials = new NetworkCredential(CuentaCorreoElectronico, ContrasenaGeneradaGmail);
                smtp.EnableSsl = true;

                foreach (var detalle in listaDetalle)
                {
                    index++;
                    if (System.IO.File.Exists(@"C:\temp\Ticket" + IdFactura + "-" + index.ToString() + ".pdf"))
                    {
                        mensaje.Attachments.Add(new Attachment(@"C:\temp\Ticket" + IdFactura + "-" + index.ToString() + ".pdf")
                        {
                            Name = "BoletoElectronico-" + IdFactura + index.ToString() + ".pdf",
                        });
                    }
                    else
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.OK, "Error, no se encontraron los boletos");
                    }
                }

                smtp.Send(mensaje);


                return new HttpStatusCodeResult(HttpStatusCode.OK, "Correo enviado correctamente");
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }


        //*****************************************************************//
        //
        //                         Detalle de la FACTURA 
        //
        //*****************************************************************//
        /*
       [HttpGet]
       public object GetFacturaDetalle(DataSourceLoadOptions loadOptions)
       {
           IServiceFacturaDetalle serviceFacturaDetalle = new ServiceFacturaDetalle();
           List<FacturaDetalle> lista = null;

           try
           {
               loadOptions.SortByPrimaryKey = false;
               lista = serviceFacturaDetalle.GetFacturaDetalle().ToList();
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
        */
        [HttpGet]
        public object GetFacturaDetalle(int IdFactura, DataSourceLoadOptions loadOptions)
        {
            IServiceFactura serviceFactura = new ServiceFactura();
            List<FacturaDetalle> lista = null;

            try
            {
                loadOptions.SortByPrimaryKey = false;
                lista = serviceFactura.GetFacturaDetalle(IdFactura).ToList();
                if (lista != null)
                {

                    var retorno = Content(JsonConvert.SerializeObject(DataSourceLoader.Load(lista, loadOptions)), "application/json");
                    return retorno;
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
        public int[] GetFacturaDetalleSecuencia(int IdFactura)
        {
            IServiceFactura serviceFactura = new ServiceFactura();
            List<FacturaDetalle> lista = serviceFactura.GetFacturaDetalle(IdFactura).ToList();
            int[] secuencias = new int[lista.Count];
            int cont = 0;

            try
            {
                if (lista != null)
                {
                    foreach (var item in lista)
                    {
                        secuencias[cont] = item.Secuencia;
                        cont++;
                    }
                    return secuencias;
                }
                else
                {
                    throw new Exception("No hay información");
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                return null;

            }
        }

        [HttpPost]
        public ActionResult PostFacturaDetalle(string values)
        {
            IServiceFactura serviceFactura = new ServiceFactura();
            FacturaDetalle oFacturaDetalle = new FacturaDetalle();
            IServiceProducto serviceProducto = new ServiceProducto();
            Producto oProducto = new Producto();

            try
            {
                JsonConvert.PopulateObject(values, oFacturaDetalle);

                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No se pudo salvar la información. [ModelState]");
                }

                //Se valida si se puede facturar la cantidad correspondiente
                if (serviceProducto.GetProductoByID(oFacturaDetalle.IdProducto).Cantidad < oFacturaDetalle.Cantidad)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "La cantidad disponible para el producto seleccionado es menor a la deseada para facturar");
                }

                if (serviceProducto.GetProductoByID(oFacturaDetalle.IdProducto).Estado.Equals(TypeEstado.INACTIVO))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No se puede selecionar el evento porque esta desabilitado");
                }

                if (serviceProducto.GetProductoByID(oFacturaDetalle.IdProducto).FechaInclusion < DateTime.Today)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No se puede selecionar el evento porque la fecha del mismo ya paso");

                }

                if (serviceFactura.SaveFacturaDetalle(oFacturaDetalle) == null)
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
        public ActionResult PutFacturaDetalle(string key, String values)
        {
            IServiceFactura serviceFactura = new ServiceFactura();
            FacturaDetalle oFacturaDetalle = new FacturaDetalle();

            try
            {
                // Crear Objeto dinámico para extraer los campos del JSON
                // Observe que ambos son los campos del Grid que son llave
                // primaria del Detalle 
                JObject parameters = JObject.Parse(key);
                int secuencia = int.Parse(parameters["Secuencia"].ToString());
                int idFactura = int.Parse(parameters["IdFactura"].ToString());

                // Buscar por Cedula
                oFacturaDetalle = serviceFactura.GetFacturaDetalleById(idFactura, secuencia);
                // Si no existe
                if (oFacturaDetalle == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, $"No existe la FacturaDetalle No. {key}");
                }
                else
                {
                    // Si existe poblar oFacturaDetalle con los values osea los properties que se actualizaron.
                    JsonConvert.PopulateObject(values, oFacturaDetalle);
                }


                // Validar el model
                if (!TryValidateModel(oFacturaDetalle))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No se pudo actualizar la información. [ModelState]");

                // Salvar / Actualizar
                if (serviceFactura.SaveFacturaDetalle(oFacturaDetalle) == null)
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
        public ActionResult DeleteFacturaDetalle(string key)
        {
            IServiceFactura serviceFactura = new ServiceFactura();
            try
            {
                // Crear Objeto dinámico para extraer los campos del JSON
                // Observe que ambos son los campos del Grid que son llave
                // primaria del Detalle 
                JObject parameters = JObject.Parse(key);
                int secuencia = int.Parse(parameters["Secuencia"].ToString());
                int idFactura = int.Parse(parameters["IdFactura"].ToString());

                serviceFactura.DeleteFacturaDetalle(idFactura, secuencia);

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }


        //*****************************************************************//
        //
        //                         Reporte de la FACTURA 
        //
        //*****************************************************************//


        [HttpGet]
        public ActionResult ImprimirFactura(int id)
        {
            string rptPath = "";
            Microsoft.Reporting.WebForms.ReportViewer rv = new Microsoft.Reporting.WebForms.ReportViewer();
            byte[] streamBytes = null;
            string mimeType = "";
            string encoding = "";
            string filenameExtension = "";
            string[] streamids = null;
            Microsoft.Reporting.WebForms.Warning[] warnings = null;
            string deviceInfo = "";
            try
            {
                // Leer parámetros
                if (id == 0)
                {
                    throw new Exception("Filtro es un dato requerido");
                }

                // Generar QR
                // Valida si el directorio existe, sino lo crea
                if (!Directory.Exists(@"C:\temp"))
                    Directory.CreateDirectory(@"C:\temp");

                // Convertir imagen a QR
                Image imagen = QuickResponse.QuickResponseGenerador(id.ToString(), 53);
                //Image imagen = QuickResponse.QuickResponseGenerador(txtFiltro, 53);
                // Salvar imagen
                imagen.Save(@"c:\temp\factura.png", ImageFormat.Png);

                // Declarar Fuente
                Reports.DSFuenteDatos dSFuenteDatos = new Reports.DSFuenteDatos();

                // Instancia el Table Adaptar ojo como se debe acceder desde el NameSpace
                Web.Reports.DSFuenteDatosTableAdapters.FacturaTableAdapter factura = new Reports.DSFuenteDatosTableAdapters.FacturaTableAdapter();
                // Llena la tabla            
                factura.Fill(dSFuenteDatos.Factura, id);
                // Colocar la ruta del reporte
                rptPath = Server.MapPath("~/Reports/rptFactura.rdlc");

                // Cargar la lista con la información
                var lista = dSFuenteDatos.Factura.ToList();

                // Primer parametro NOMBRE del DataSet que se le coloco en el REPORT
                Microsoft.Reporting.WebForms.ReportDataSource rds
                    = new Microsoft.Reporting.WebForms.ReportDataSource("DataSet", lista);

                rv.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                rv.LocalReport.ReportPath = rptPath;
                rv.LocalReport.DataSources.Add(rds);
                rv.LocalReport.EnableHyperlinks = true;

                // Parametros del QR
                // Config imagen del QR
                string ruta = @"file:///" + @"C:/TEMP/factura.png";
                // Habilitar imagenes externas
                rv.LocalReport.EnableExternalImages = true;
                // Pasar parámetro
                ReportParameter param = new ReportParameter("quickresponse", ruta);
                rv.LocalReport.SetParameters(param);


                rv.LocalReport.Refresh();
                deviceInfo = "<DeviceInfo>" +
                "  <OutputFormat>PDF</OutputFormat>" +
                "  <PageWidth>8.5in</PageWidth>" +
                "  <PageHeight>11in</PageHeight>" +
                "  <MarginTop>0.5in</MarginTop>" +
                "  <MarginLeft>1in</MarginLeft>" +
                "  <MarginRight>1in</MarginRight>" +
                "  <MarginBottom>0.5in</MarginBottom>" +
                "  <EmbedFonts>None</EmbedFonts>" +
                "</DeviceInfo>";

                streamBytes = rv.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

                System.IO.File.WriteAllBytes(@"C:\temp\factura" + id + ".pdf", streamBytes);

                ImprimirBoleto(id);
                // Retorna un pdf
                return File(streamBytes, "application/pdf");
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = ex.Message;
                TempData["Type"] = "Fail";
                return View("Index");
            }
        }

        [HttpGet]
        public ActionResult ImprimirBoleto(int IdFactura)
        {
            string rptPath = "";
            Microsoft.Reporting.WebForms.ReportViewer rv = new Microsoft.Reporting.WebForms.ReportViewer();
            Microsoft.Reporting.WebForms.ReportViewer rv2 = new Microsoft.Reporting.WebForms.ReportViewer();
            byte[] streamBytes = null;
            string mimeType = "";
            string encoding = "";
            string filenameExtension = "";
            string[] streamids = null;
            Microsoft.Reporting.WebForms.Warning[] warnings = null;
            string deviceInfo = "";
            int index = 0;
            int cont = 0;

            IServiceFactura serviveFactura = new ServiceFactura();
            IServiceCliente serviceCliente = new ServiceCliente();
            int[] listaSecuencias = GetFacturaDetalleSecuencia(IdFactura);


            try
            {
                // Leer parámetros
                if (IdFactura == 0)
                {
                    throw new Exception("Filtro es un dato requerido");
                }
                else
                {

                    // Generar QR
                    // Valida si el directorio existe, sino lo crea
                    if (!Directory.Exists(@"C:\temp"))
                        Directory.CreateDirectory(@"C:\temp");

                    int secuencia = serviveFactura.GetFacturaEncabezadoById(IdFactura).IdFactura;


                    // Convertir imagen a QR
                    Image imagen = QuickResponse.QuickResponseGenerador(secuencia.ToString(), 53);
                    //Image imagen = QuickResponse.QuickResponseGenerador(txtFiltro, 53);
                    // Salvar imagen
                    imagen.Save(@"c:\temp\Ticket.png", ImageFormat.Png);
                    index = 1; ;
                    foreach(var item in listaSecuencias){
                        if (System.IO.File.Exists(@"C:\temp\Ticket" + IdFactura + "-" + index.ToString() + listaSecuencias[cont] + ".pdf") == false)
                        {
                            Reports.DSFuenteDatos dSFuenteDatos = new Reports.DSFuenteDatos();

                            Web.Reports.DSFuenteDatosTableAdapters.TicketTableAdapter ticket = new Reports.DSFuenteDatosTableAdapters.TicketTableAdapter();
                            // Llena la tabla            
                            ticket.Fill(dSFuenteDatos.Ticket, listaSecuencias[cont]);

                            // Colocar la ruta del reporte
                            rptPath = Server.MapPath("~/Reports/rptBoletoElectronico.rdlc");

                            // Cargar la lista con la información
                            var lista = dSFuenteDatos.Ticket.ToList();

                            // Primer parametro NOMBRE del DataSet que se le coloco en el REPORT
                            Microsoft.Reporting.WebForms.ReportDataSource rds
                                = new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", lista);

                            rv.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                            rv.LocalReport.ReportPath = rptPath;
                            rv.LocalReport.DataSources.Add(rds);
                            rv.LocalReport.EnableHyperlinks = true;


                            // Parametros del QR
                            // Config imagen del QR
                            string ruta = @"file:///" + @"C:/TEMP/Ticket.png";
                            // Habilitar imagenes externas
                            rv.LocalReport.EnableExternalImages = true;
                            // Pasar parámetro
                            ReportParameter param = new ReportParameter("quickresponse", ruta);
                            rv.LocalReport.SetParameters(param);


                            rv.LocalReport.Refresh();
                            deviceInfo = "<DeviceInfo>" +
                            "  <OutputFormat>PDF</OutputFormat>" +
                            "  <PageWidth>8.5in</PageWidth>" +
                            "  <PageHeight>11in</PageHeight>" +
                            "  <MarginTop>0.5in</MarginTop>" +
                            "  <MarginLeft>1in</MarginLeft>" +
                            "  <MarginRight>1in</MarginRight>" +
                            "  <MarginBottom>0.5in</MarginBottom>" +
                            "  <EmbedFonts>None</EmbedFonts>" +
                            "</DeviceInfo>";

                            streamBytes = rv.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                            System.IO.File.WriteAllBytes(@"C:\temp\Ticket" + IdFactura + "-" + index.ToString() + ".pdf", streamBytes);
                            cont++;
                            index++;
                            continue;
                        }
                        else
                        {
                            Reports.DSFuenteDatos dSFuenteDatos = new Reports.DSFuenteDatos();

                            Web.Reports.DSFuenteDatosTableAdapters.TicketTableAdapter boleto = new Reports.DSFuenteDatosTableAdapters.TicketTableAdapter();
                            // Llena la tabla            
                            boleto.Fill(dSFuenteDatos.Ticket, listaSecuencias[cont]);

                            // Colocar la ruta del reporte
                            rptPath = Server.MapPath("~/Reports/rptBoletoElectronico.rdlc");

                            // Cargar la lista con la información
                            var lista = dSFuenteDatos.Ticket.ToList();

                            // Primer parametro NOMBRE del DataSet que se le coloco en el REPORT
                            Microsoft.Reporting.WebForms.ReportDataSource rds
                                = new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", lista);

                            rv2.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                            rv2.LocalReport.ReportPath = rptPath;
                            rv2.LocalReport.DataSources.Add(rds);
                            rv2.LocalReport.EnableHyperlinks = true;


                            // Parametros del QR
                            // Config imagen del QR
                            string ruta = @"file:///" + @"C:/TEMP/Ticket.png";
                            // Habilitar imagenes externas
                            rv2.LocalReport.EnableExternalImages = true;
                            // Pasar parámetro
                            ReportParameter param = new ReportParameter("quickresponse", ruta);
                            rv2.LocalReport.SetParameters(param);


                            rv2.LocalReport.Refresh();
                            deviceInfo = "<DeviceInfo>" +
                            "  <OutputFormat>PDF</OutputFormat>" +
                            "  <PageWidth>8.5in</PageWidth>" +
                            "  <PageHeight>11in</PageHeight>" +
                            "  <MarginTop>0.5in</MarginTop>" +
                            "  <MarginLeft>1in</MarginLeft>" +
                            "  <MarginRight>1in</MarginRight>" +
                            "  <MarginBottom>0.5in</MarginBottom>" +
                            "  <EmbedFonts>None</EmbedFonts>" +
                            "</DeviceInfo>";

                            streamBytes = rv2.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                            System.IO.File.WriteAllBytes(@"C:\temp\Ticket" + IdFactura + "-" + (index + 1).ToString() + ".pdf", streamBytes);
                            cont++;
                            index++;
                            continue;
                        }
                    }
                    
                    return File(streamBytes, "application/pdf");
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

    }
}