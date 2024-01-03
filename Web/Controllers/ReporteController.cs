using ApplicationCore.Services;
using Infraestructure.Models.Catalogo;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Web.Reports;

namespace Web.Controllers
{
    public class ReporteController : Controller
    {
        // GET: Reporte
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ReporteClientes()
        {
            string rptPath = "";
            ReportViewer rv = new ReportViewer();
            byte[] streamBytes = null;
            string mimeType = "";
            string encoding = "";
            string filenameExtension = "";
            string[] streamids = null;
            Warning[] warnings = null;
            string deviceInfo = "";


            // Declarar Fuente
            DSFuenteDatos dSFuenteDatos = new DSFuenteDatos();

            try
            {

                // Instancia el Table Adaptar ojo como se debe acceder desde el NameSpace
                Web.Reports.DSFuenteDatosTableAdapters.ClienteTableAdapter clienteTableAdapter = new Reports.DSFuenteDatosTableAdapters.ClienteTableAdapter();
                // Llena la tabla            
                clienteTableAdapter.Fill(dSFuenteDatos.Cliente);
                // Colocar la ruta del reporte
                rptPath = Server.MapPath("~/Reports/rptCliente.rdlc");

                var lista = dSFuenteDatos.Cliente.ToList();

                // Primer parametro NOMBRE del DataSet que se le coloco en el REPORT
                ReportDataSource rds = new ReportDataSource("DataSet", lista);

                rv.ProcessingMode = ProcessingMode.Local;
                rv.LocalReport.ReportPath = rptPath;
                rv.LocalReport.DataSources.Add(rds);
                rv.LocalReport.EnableHyperlinks = true;
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

        [HttpPost]
        public ActionResult ReporteClienteXCalidades(FormCollection form)
        {
            string rptPath = "";
            ReportViewer rv = new ReportViewer();
            byte[] streamBytes = null;
            string mimeType = "";
            string encoding = "";
            string filenameExtension = "";
            string[] streamids = null;
            Warning[] warnings = null;
            string deviceInfo = "";
            string txtFiltro = "";
           

            try
            {
                // Leer parámetros
                txtFiltro = form["txtFiltro"].ToString();

                if (string.IsNullOrEmpty(txtFiltro))
                {
                    throw new Exception("Filtro es un dato requerido");
                }

                // Agregar % para el Query en SQL

                txtFiltro = txtFiltro.Replace(' ', '%');
                txtFiltro = "%" + txtFiltro + "%";

                // Declarar Fuente
                DSFuenteDatos dSFuenteDatos = new DSFuenteDatos();

                // Instancia el Table Adaptar ojo como se debe acceder desde el NameSpace
                Web.Reports.DSFuenteDatosTableAdapters.ClienteCalidadesTableAdapter clienteTableAdapter = new Reports.DSFuenteDatosTableAdapters.ClienteCalidadesTableAdapter();
                // Llena la tabla            
                clienteTableAdapter.Fill(dSFuenteDatos.ClienteCalidades, txtFiltro);
                // Colocar la ruta del reporte
                rptPath = Server.MapPath("~/Reports/rptClienteCalidades.rdlc");

                var lista = dSFuenteDatos.ClienteCalidades.ToList();

                // Primer parametro NOMBRE del DataSet que se le coloco en el REPORT
                ReportDataSource rds = new ReportDataSource("DataSet", lista);

                rv.ProcessingMode = ProcessingMode.Local;
                rv.LocalReport.ReportPath = rptPath;
                rv.LocalReport.DataSources.Add(rds);
                rv.LocalReport.EnableHyperlinks = true;
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


        public ActionResult ReporteEtiquetaProducto()
        {
            return View("ReporteEtiquetaProducto");
        }

        [HttpPost]
        public ActionResult ReporteEtiquetaProducto(FormCollection form)
        {
            string rptPath = "";
            ReportViewer rv = new ReportViewer();
            byte[] streamBytes = null;
            string mimeType = "";
            string encoding = "";
            string filenameExtension = "";
            string[] streamids = null;
            Warning[] warnings = null;
            string deviceInfo = "";
            string txtFiltro = "";
            int idProducto  = 0;

            IServiceProducto serviceProducto = new ServiceProducto();

            try
            {
                // Leer parámetros
                txtFiltro = form["txtFiltro"].ToString();

                if (string.IsNullOrEmpty(txtFiltro))
                {
                    throw new Exception("Filtro es un dato requerido");
                }

                if (!int.TryParse(txtFiltro,out   idProducto))
                {
                    throw new Exception("El código es numérico");
                }

                 
                Producto oProducto = serviceProducto.GetProductoByID(idProducto);

                if (oProducto == null)
                {
                    throw new Exception($"El código {idProducto} NO existe!");
                }

                 
                // Valida si el directorio existe, sino lo crea
                if (!Directory.Exists(@"C:\temp"))
                    Directory.CreateDirectory(@"C:\temp");

                // Convertir imagen a QR
                Image imagen = QuickResponse.QuickResponseGenerador(oProducto.IdTipo + " - " + oProducto.DescripcionElectronico , 53);
                //Image imagen = QuickResponse.QuickResponseGenerador(txtFiltro, 53);
                // Salvar imagen
                imagen.Save(@"c:\temp\qr.png", ImageFormat.Png);

                // Declarar Fuente
                DSFuenteDatos dSFuenteDatos = new DSFuenteDatos();

                // Instancia el Table Adaptar ojo como se debe acceder desde el NameSpace
                Web.Reports.DSFuenteDatosTableAdapters.ProductoEtiquetaTableAdapter productoTableAdapter = new Reports.DSFuenteDatosTableAdapters.ProductoEtiquetaTableAdapter();
                // Llena la tabla

                productoTableAdapter.Fill(dSFuenteDatos.ProductoEtiqueta, Convert.ToInt32(txtFiltro));
                // Colocar la ruta del reporte
                rptPath = Server.MapPath("~/Reports/rptProductoEtiqueta.rdlc");

                var lista = dSFuenteDatos.ProductoEtiqueta.ToList();

                // Primer parametro NOMBRE del DataSet que se le coloco en el REPORT
                ReportDataSource rds = new ReportDataSource("DataSet", lista);                               

                //****************************************************************
                // Debe tener exactamente el mismo ORDEN de acá en adelante.
                rv.ProcessingMode = ProcessingMode.Local;
                rv.LocalReport.ReportPath = rptPath;
                rv.LocalReport.DataSources.Add(rds);
                rv.LocalReport.EnableHyperlinks = true;


                // Config imagen del QR
                string ruta = @"file:///" + @"C:/TEMP/qr.png";
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
    }
}