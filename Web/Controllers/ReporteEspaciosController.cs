using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Web.Reports;

namespace Web.Controllers
{
    public class ReporteEspaciosController : Controller
    {
        // GET: ReporteEspacios
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReporteEspacios(FormCollection form)
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

            DateTime txtFechas;
            int txtID;


            try
            {
                // Leer parámetros
                txtFechas = Convert.ToDateTime(form["txtFechas"]);
                txtID = Convert.ToInt32(form["txtID"]);

                //if (string.IsNullOrEmpty(txtFechas))
                //{
                //    throw new Exception("Filtro es un dato requerido");
                //}

                // Agregar % para el Query en SQL

                //txtFechas = txtFechas.Replace(' ', '%');
                //txtFechas = "%" + txtFechas + "%";

                // Declarar Fuente
                DSFuenteDatos dSFuenteDatos = new DSFuenteDatos();

                // Instancia el Table Adaptar ojo como se debe acceder desde el NameSpace
                Web.Reports.DSFuenteDatosTableAdapters.ProductoTableAdapter clienteTableAdapter = new Reports.DSFuenteDatosTableAdapters.ProductoTableAdapter();
                // Llena la tabla            
                clienteTableAdapter.Fill(dSFuenteDatos.Producto);
                // Colocar la ruta del reporte
                rptPath = Server.MapPath("~/Reports/rptEspacio.rdlc");

                var lista = dSFuenteDatos.Producto.ToList();

                // Primer parametro NOMBRE del DataSet que se le coloco en el REPORT
                ReportDataSource rds = new ReportDataSource("DataSet1", lista);

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

    }
}