using Infraestructure.Models;
using Infraestructure.Models.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
   public   interface IRepositoryProducto
    {
        IEnumerable<Producto> GetProducto();
        IEnumerable<Producto> GetProductoByName(String name);
        Producto GetProductoByID(int id);
        void DeleteProducto(int id);
        Producto Save(Producto producto);

    }
}
