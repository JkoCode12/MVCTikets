using Infraestructure.Models;
using Infraestructure.Models.Catalogo;
using Infraestructure.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
    public class RepositoryProducto : IRepositoryProducto
    {
         
        public void DeleteProducto(int id)
        {
            int returno;
            using ( DataModel ctx = new DataModel())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                Producto oProducto = new Producto()
                {
                     IdProducto = id
                };
                ctx.Entry(oProducto).State = EntityState.Deleted;
                returno = ctx.SaveChanges();
            }
        }
 
        public IEnumerable<Producto> GetProducto()
        {
            IEnumerable<Producto> lista = null;
            using (DataModel ctx = new DataModel())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                lista = ctx.Producto.ToList();
            }

            return lista;
        }

        public Producto GetProductoByID(int id)
        {
            Producto oProducto = null;
            using (DataModel ctx = new DataModel())
            {
                ctx.Configuration.LazyLoadingEnabled = false;

                oProducto = ctx.Producto.Where(p => p.IdProducto == id).FirstOrDefault();
 
            }

            return oProducto;
        }



        public IEnumerable<Producto> GetProductoByName(string name)
        {

            IEnumerable<Producto> lista = null;
            using (DataModel ctx = new DataModel())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                lista = ctx.Producto.ToList<Producto>().
                                                      FindAll(p => p.DescripcionElectronico.ToLower().
                                                      Contains(name.ToLower()));
            }
            return lista; 
        }


        public Producto Save(Producto producto)
        {
            int retorno = 0;
            Producto oProducto = null;

            using (DataModel ctx = new DataModel())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                oProducto = GetProductoByID((int)producto.IdProducto);
                if (oProducto == null)
                {
                    ctx.Producto.Add(producto);
                }
                else
                {
                    ctx.Entry(producto).State = EntityState.Modified;
                }
                retorno = ctx.SaveChanges();
            }

            if (retorno >= 0)
                oProducto = GetProductoByID((int)producto.IdProducto);

            return oProducto;
        }
    }
}
