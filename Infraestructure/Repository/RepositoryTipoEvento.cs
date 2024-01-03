using Infraestructure.Models;
using Infraestructure.Models.Catalogo;
using Infraestructure.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace Infraestructure.Repository
{

    public class RepositoryTipoEvento : IRepositoryTipoEvento
    {

        public void DeleteTipoEvento(int id)
        {
            int returno;
            try
            {

                using (DataModel ctx = new DataModel())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    TipoEvento tipo = new TipoEvento()
                    {
                        IdTipo = id
                    };
                    ctx.Entry(tipo).State = EntityState.Deleted;
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

        public IEnumerable<TipoEvento> GetTipoEvento()
        {

            try
            {
                IEnumerable<TipoEvento> lista = null;
                using (DataModel ctx = new DataModel())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    lista = ctx.TipoEvento.ToList<TipoEvento>();
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

        

        public TipoEvento GetTipoEventoByID(int id)
        {
            TipoEvento tipo = null;
            try
            {

                using (DataModel ctx = new DataModel())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    tipo = ctx.TipoEvento.Find(id);
                }

                return tipo;
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

       
        public TipoEvento Save(TipoEvento tipo)
        {
            int retorno = 0;
            TipoEvento oTipo = null;
            try
            {
                using (DataModel ctx = new DataModel())
                {

                    ctx.Configuration.LazyLoadingEnabled = false;
                    oTipo = GetTipoEventoByID(tipo.IdTipo);
                    if (oTipo == null)
                    {
                        ctx.TipoEvento.Add(tipo);
                    }
                    else
                    {
                        ctx.Entry(tipo).State = EntityState.Modified;
                    }
                    retorno = ctx.SaveChanges();


#if !DEBUG
                   // Error por exception
                    int x = 0;
                    x = 25 / x; 

                    // Error por Entity Framework 
                    // Forzar un error por duplicidad
                   // tipo.IdTipo = 1;
                   // ctx.TipoEvento.Add(tipo);
                   // retorno = ctx.SaveChanges();
#endif
                }

                if (retorno >= 0)
                    oTipo = GetTipoEventoByID(tipo.IdTipo);

                return oTipo;
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

        public bool ActivateDeactivate(int id)
        {
            TipoEvento oTipo = null;
            int retorno = 0;
            try
            {
                using (DataModel ctx = new DataModel())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oTipo = ctx.TipoEvento.Find(id);

                    // Si es inactivo Activelo
                    if (oTipo.Estado == TypeEstado.INACTIVO)
                    {
                        oTipo.Estado = TypeEstado.ACTIVO;
                    }
                    else
                    {
                        oTipo.Estado = TypeEstado.INACTIVO;
                    }
                    ctx.Entry(oTipo).State = EntityState.Modified;

                    retorno = ctx.SaveChanges();
                } 

                return retorno > 0 ? true : false;
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
    }
}