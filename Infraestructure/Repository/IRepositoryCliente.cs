using Infraestructure.Models;
using Infraestructure.Models.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
    public interface IRepositoryCliente
    {
        IEnumerable<Cliente> GetCliente();
        IEnumerable<Cliente> GetClienteByName(String name);
        Cliente GetClienteByID(string id);
        void DeleteCliente(string id);
        Cliente Save(Cliente cliente);


    }
}
