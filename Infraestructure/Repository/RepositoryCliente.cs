using Infraestructure.Models;
using Infraestructure.Models.Catalogo;
using Infraestructure.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
    public class RepositoryCliente : IRepositoryCliente
    {
        public void DeleteCliente(string id)
        {
            int returno;
            try
            {
                using (DataModel ctx = new DataModel())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    Cliente cliente = new Cliente()
                    {
                         IdCliente = id
                    };
                    ctx.Entry(cliente).State = EntityState.Deleted;
                    returno = ctx.SaveChanges();
                }
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public IEnumerable<Cliente> GetCliente()
        {
            IEnumerable<Cliente> lista = null;
            try
            {
                using (DataModel ctx = new DataModel())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    lista = ctx.Cliente.ToList();
                }

                return lista;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public Cliente GetClienteByID(string id)
        {
            Cliente oCliente = null;
            try
            {
                using (DataModel ctx = new DataModel())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oCliente = ctx.Cliente.Find(id);
                }

                return oCliente;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public IEnumerable<Cliente> GetClienteByName(string name)
        {
            IEnumerable<Cliente> lista = null;
            try
            {
                string sql =
                    string.Format("select * from Cliente where Nombre+Apellido1+Apellido2 like  '%{0}%' ", name);
                using (DataModel ctx = new DataModel())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;

                    lista = ctx.Cliente.SqlQuery(sql).ToList<Cliente>();
                }

                return lista;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }
        
        public Cliente Save(Cliente cliente)
        {
            int retorno = 0;
            Cliente oCliente = null; 
             
            using (DataModel ctx = new DataModel())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                 oCliente = GetClienteByID(cliente.IdCliente);
                if (oCliente == null)
                {
                    
                    ctx.Cliente.Add(cliente);
                }
                else
                {
                    ctx.Entry(cliente).State = EntityState.Modified;
                }
                retorno = ctx.SaveChanges();
            }

            if (retorno >= 0)
                oCliente = GetClienteByID(cliente.IdCliente);

            return oCliente;
        }
    }
}
