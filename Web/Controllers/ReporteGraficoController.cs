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
    public class ReporteGraficoController : Controller
    {
        // GET: ReporteGrafico
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReporteGrafico(FormCollection form)
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
            DateTime txtFechas2;


            try
            {
                // Leer parámetros
                txtFechas = Convert.ToDateTime(form["txtFechas"]);
                txtFechas2 = Convert.ToDateTime(form["txtFechas2"]);

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
                Web.Reports.DSFuenteDatosTableAdapters.ReporteGraficoTableAdapter graficoTableAdapter = new Reports.DSFuenteDatosTableAdapters.ReporteGraficoTableAdapter();
                // Llena la tabla            
                graficoTableAdapter.Fill(dSFuenteDatos.ReporteGrafico, txtFechas, txtFechas2);
                // Colocar la ruta del reporte
                rptPath = Server.MapPath("~/Reports/rptGrafico.rdlc");

                var lista = dSFuenteDatos.ReporteGrafico.ToList();

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