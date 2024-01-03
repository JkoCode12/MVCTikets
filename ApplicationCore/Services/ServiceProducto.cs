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
    public class ServiceProducto : IServiceProducto
    {
        public void DeleteProducto(int id)
        {
            IRepositoryProducto repository = new RepositoryProducto();
            repository.DeleteProducto(id);
        }

        public IEnumerable<Producto> GetProducto()
        {
            IRepositoryProducto repository = new RepositoryProducto();
            return repository.GetProducto();
        }

        public Producto GetProductoByIDWithInventary(int id, int pCantidad, ref bool pError, ref string pMensaje)
        {
            IRepositoryProducto repository = new RepositoryProducto();

            var oProducto = repository.GetProductoByID(id);

            if (oProducto == null) {
                pError = true;
                pMensaje = $"Código de Producto {id} No existe  ";
                return null;
            }

            if (oProducto.Cantidad == 0)
            {
                pError = true;
                pMensaje = $"No hay inventario disponible para {oProducto.DescripcionElectronico}";
                return null;
            }

            if (pCantidad > oProducto.Cantidad  )
            {
                pError = true;
                pMensaje = $"Cantidad insuficiente en inventario, solo hay {oProducto.Cantidad} ";
                return null;
            } 

            return oProducto;
        }


        public Producto GetProductoByID(int id)
        {
            IRepositoryProducto repository = new RepositoryProducto();
            return repository.GetProductoByID(id);
        }

        public IEnumerable<Producto> GetProductoByName(string name)
        {
            IRepositoryProducto repository = new RepositoryProducto();
            return repository.GetProductoByName(name);
        }

        public Producto Save(Producto producto)
        {
            IRepositoryProducto repository = new RepositoryProducto();
            return repository.Save(producto);
        }


    }
}
