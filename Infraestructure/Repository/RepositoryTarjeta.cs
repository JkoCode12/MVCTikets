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

    public class RepositoryTarjeta : IRepositoryTarjeta
    {
        public void DeleteTarjeta(int id)
        {
            int returno;
            try
            {
                using (DataModel ctx = new DataModel())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    Tarjeta Tarjeta = new Tarjeta()
                    {
                        IdTarjeta = id
                    };
                    Log.Info("Se ingresa a eliminar la tarjeta número " + Tarjeta.IdTarjeta);
                    ctx.Entry(Tarjeta).State = EntityState.Deleted;
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

        public IEnumerable<Tarjeta> GetTarjeta()
        {
            try
            {
                IEnumerable<Tarjeta> lista = null;
                using (DataModel ctx = new DataModel())
                {
                    ctx.Configuration.LazyLoadingEnabled = true;
                    lista = ctx.Tarjeta.ToList<Tarjeta>();
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

        public Tarjeta GetTarjetaByID(int id)
        {
            Tarjeta Tarjeta = null;
            try
            {
                using (DataModel ctx = new DataModel())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    Tarjeta = ctx.Tarjeta.Find(id);
                    //Tarjeta = ctx.Tarjeta.Where(x => x.IdTarjeta == id).FirstOrDefault();
                }
                return Tarjeta;
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

        public Tarjeta Save(Tarjeta tarjeta)
        {
            int retorno = 0;
            Tarjeta oTarjeta = null;
            try
            {
                using (DataModel ctx = new DataModel())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oTarjeta = GetTarjetaByID(tarjeta.IdTarjeta);
                    if (oTarjeta == null)
                    {
                        ctx.Tarjeta.Add(tarjeta);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        ctx.Entry(tarjeta).State = EntityState.Modified;
                        ctx.SaveChanges();
                    }
                    retorno = ctx.SaveChanges();
                }

                if (retorno >= 0)
                    oTarjeta = GetTarjetaByID(tarjeta.IdTarjeta);

                return oTarjeta;
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