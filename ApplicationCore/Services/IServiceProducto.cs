using Infraestructure.Models;
using Infraestructure.Models.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IServiceProducto
    {
        IEnumerable<Producto> GetProducto();
        IEnumerable<Producto> GetProductoByName(String name);
        Producto GetProductoByID(int id);
        void DeleteProducto(int id);
        Producto Save(Producto producto);
        Producto GetProductoByIDWithInventary(int id, int pCantidad, ref bool pError, ref string pMensaje);
    }
}
