using Infraestructure.Models.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
    public interface IRepositoryFactura
    {
        /**************************  Encabezado ***********************/
        IEnumerable<FacturaEncabezado> GetFacturaEncabezado();
        IEnumerable<FacturaEncabezado> GetFacturaEncabezadoPendientes();
        IEnumerable<FacturaEncabezado> GetFacturaEncabezadoConsulta(DateTime FechaInicial, DateTime FechaFinal, int pEstadoFactura);
        FacturaEncabezado GetFacturaEncabezadoById(int id);
        void DeleteFacturaEncabezado(int id);
        FacturaEncabezado SaveFacturaEncabezado(FacturaEncabezado facturaEncabezado);
        bool Facturar(int id);

        /**************************    Detalle  **************************/
        IEnumerable<FacturaDetalle> GetFacturaDetalle();

        IEnumerable<FacturaDetalle> GetFacturaDetalle(int id);

        FacturaDetalle GetFacturaDetalleById(int idFactura, int secuenci);
        void DeleteFacturaDetalle(int idFactura, int secuencia);
        FacturaDetalle SaveFacturaDetalle(FacturaDetalle facturaDetalle);
    }
}
