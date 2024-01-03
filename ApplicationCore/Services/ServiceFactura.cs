using Infraestructure.Models.Catalogo;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceFactura : IServiceFactura
    {

        //*******************   Encabezado  ***********************//
        public FacturaEncabezado SaveFacturaEncabezado(FacturaEncabezado facturaEncabezado)
        {
            IRepositoryFactura repository = new RepositoryFactura();
            return repository.SaveFacturaEncabezado(facturaEncabezado);
        }
        public IEnumerable<FacturaEncabezado> GetFacturaEncabezado()
        {
            IRepositoryFactura repository = new RepositoryFactura();
            return repository.GetFacturaEncabezado();
        }

        public FacturaEncabezado GetFacturaEncabezadoById(int id)
        {
            IRepositoryFactura repository = new RepositoryFactura();
            return repository.GetFacturaEncabezadoById(id);
        }

        public IEnumerable<FacturaEncabezado> GetFacturaEncabezadoConsulta(DateTime fechaInicial, DateTime fechaFinal, int pEstadoFactura)
        {
            IRepositoryFactura repository = new RepositoryFactura();
            return repository.GetFacturaEncabezadoConsulta(fechaInicial, fechaFinal, pEstadoFactura);
        }

        public IEnumerable<FacturaEncabezado> GetFacturaEncabezadoPendientes()
        {
            IRepositoryFactura repository = new RepositoryFactura();
            return repository.GetFacturaEncabezadoPendientes();
        }
        public bool Facturar(int id)
        {
            IRepositoryFactura repository = new RepositoryFactura();
            return repository.Facturar(id);
        }
        public void DeleteFacturaEncabezado(int id)
        {
            IRepositoryFactura repository = new RepositoryFactura();
            repository.DeleteFacturaEncabezado(id);
        }

        //*******************   Detalle  ***********************//
        
        public void DeleteFacturaDetalle(int idFactura, int secuencia)
        {
            IRepositoryFactura repository = new RepositoryFactura();
            repository.DeleteFacturaDetalle(  idFactura,   secuencia);
        }

        public IEnumerable<FacturaDetalle> GetFacturaDetalle()
        {
            IRepositoryFactura repository = new RepositoryFactura();
            return repository.GetFacturaDetalle();

        }


        public IEnumerable<FacturaDetalle> GetFacturaDetalle(int id)
        {
            IRepositoryFactura repository = new RepositoryFactura();
            return repository.GetFacturaDetalle(id);
        }

        public FacturaDetalle GetFacturaDetalleById(int idFactura, int secuencia)
        {
            IRepositoryFactura repository = new RepositoryFactura();
            return repository.GetFacturaDetalleById(  idFactura,   secuencia);
        }
        
        public FacturaDetalle SaveFacturaDetalle(FacturaDetalle facturaDetalle)
        {
            IRepositoryFactura repository = new RepositoryFactura();
            return repository.SaveFacturaDetalle(facturaDetalle);
        }

    }
}
