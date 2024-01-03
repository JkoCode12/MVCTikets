using Infraestructure.Models;
using Infraestructure.Models.Catalogo;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{ 
    public class ServiceTarjeta : IServiceTarjeta
    {
         
        public void DeleteTarjeta(int id)
        {
            IRepositoryTarjeta repository = new RepositoryTarjeta();
            repository.DeleteTarjeta(id);
        }

        public IEnumerable<Tarjeta> GetTarjeta()
        {
            IRepositoryTarjeta repository = new RepositoryTarjeta();
            return repository.GetTarjeta();
        }

        public Tarjeta GetTarjetaByID(int id)
        {
            RepositoryTarjeta repository = new RepositoryTarjeta();
            return repository.GetTarjetaByID(id);
        }

        public Tarjeta Save(Tarjeta tarjeta)
        {
            RepositoryTarjeta repository = new RepositoryTarjeta();
            
            // Business rules
            if (tarjeta.DescripcionTarjeta.ToString().ToUpper().StartsWith("AMERICAN")) {
                throw new Exception("No se permite las tarjetas de Amercian Express");
            }

            
            return repository.Save(tarjeta);
        }
    }
}