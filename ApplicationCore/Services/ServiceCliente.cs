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
    public class ServiceCliente : IServiceCliente
    {
        public void DeleteCliente(string id)
        {
            IRepositoryCliente repository = new RepositoryCliente();
            repository.DeleteCliente(id);
        }

        public IEnumerable<Cliente> GetCliente()
        {
            IRepositoryCliente repository = new RepositoryCliente();
            return repository.GetCliente();
        }

        public Cliente GetClienteByID(string id)
        {
            IRepositoryCliente repository = new RepositoryCliente();
            return repository.GetClienteByID(id);
        }
 
         
        public IEnumerable<Cliente> GetClienteByName(string name)
        {
            IRepositoryCliente repository = new RepositoryCliente();
            return repository.GetClienteByName(name);
        }

        public Cliente Save(Cliente cliente)
        {

            IRepositoryCliente repository = new RepositoryCliente();
            return repository.Save(cliente);
            
        }
    }
}
