using Infraestructure.Models.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IServiceFactura
    {
        /**************************  Encabezado ***********************/
        IEnumerable<FacturaEncabezado> GetFacturaEncabezado();
        IEnumerable<FacturaEncabezado> GetFacturaEncabezadoPendientes();
        IEnumerable<FacturaEncabezado> GetFacturaEncabezadoConsulta(DateTime fechaInicial, DateTime fechaFinal, int pEstadoFactura);
        FacturaEncabezado GetFacturaEncabezadoById(int id);
        void DeleteFacturaEncabezado(int id);
        FacturaEncabezado SaveFacturaEncabezado(FacturaEncabezado facturaEncabezado);
        bool Facturar(int id);

        /**************************    Detalle  **************************/
        IEnumerable<FacturaDetalle> GetFacturaDetalle();

        IEnumerable<FacturaDetalle> GetFacturaDetalle(int id);

        FacturaDetalle GetFacturaDetalleById(int idFactura, int secuencia);
        void DeleteFacturaDetalle(int idFactura, int secuencia);
        FacturaDetalle SaveFacturaDetalle(FacturaDetalle facturaDetalle);
    }
}
