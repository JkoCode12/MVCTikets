using Infraestructure.Models.Catalogo;
using Infraestructure.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
    public class RepositoryFactura : IRepositoryFactura
    {
        // *******************  Encabezado ***********************//
        public void DeleteFacturaEncabezado(int id)
        {
            int returno;
            try
            {
                using (DataModel ctx = new DataModel())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    //FacturaEncabezado facturaEncabezado = new FacturaEncabezado()
                    //{
                    //    IdFactura = id
                    //};
                    //ctx.Entry(facturaEncabezado).State = EntityState.Deleted;

                    FacturaEncabezado facturaEncabezado = new FacturaEncabezado();
                    facturaEncabezado = ctx.FacturaEncabezado.Where(x => x.IdFactura == id).FirstOrDefault();
                    facturaEncabezado.EstadoFactura = TypeEstadoFactura.ANULADA;
                    ctx.Entry(facturaEncabezado).State = EntityState.Modified;
                    returno = ctx.SaveChanges();
                    //Se obtiene la lista de Detalles para agregar los valores al inventario
                    List<FacturaDetalle> facturaDetalles = new List<FacturaDetalle>();
                    facturaDetalles = ctx.FacturaDetalle.Where(x => x.IdFactura == facturaEncabezado.IdFactura).ToList();
                    if (facturaDetalles != null)
                    {
                        foreach (var item in facturaDetalles)
                        {
                            Producto producto = new Producto();
                            producto = ctx.Producto.Where(x => x.IdProducto == item.IdProducto).FirstOrDefault();
                            producto.Cantidad = producto.Cantidad + item.Cantidad;
                            ctx.Entry(facturaEncabezado).State = EntityState.Modified;
                            ctx.SaveChanges();
                        }
                    }
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

        public bool Facturar(int id)
        {
            FacturaEncabezado oFacturaEncabezado = null;
            int retorno = 0;
            try
            {
                using (DataModel ctx = new DataModel())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oFacturaEncabezado = ctx.FacturaEncabezado.Find(id);
                    oFacturaEncabezado.EstadoFactura = TypeEstadoFactura.PROCESADA;
                    ctx.Entry(oFacturaEncabezado).State = EntityState.Modified;
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

        public IEnumerable<FacturaEncabezado> GetFacturaEncabezado()
        {
            List<FacturaEncabezado> lista = null;
            try
            {
                using (DataModel ctx = new DataModel())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    lista = ctx.FacturaEncabezado.OrderByDescending(x => x.IdFactura).ToList();
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

        public FacturaEncabezado GetFacturaEncabezadoById(int id)
        {
            FacturaEncabezado facturaEncabezado = null;
            try
            {

                using (DataModel ctx = new DataModel())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    facturaEncabezado = ctx.FacturaEncabezado.
                              Where(p => p.IdFactura == id).
                              FirstOrDefault<FacturaEncabezado>();
                }

                return facturaEncabezado;
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

        public IEnumerable<FacturaEncabezado> GetFacturaEncabezadoConsulta(DateTime FechaInicial, DateTime FechaFinal, int pEstadoFactura)
        {
            try
            {
                IEnumerable<FacturaEncabezado> lista = null;
                using (DataModel ctx = new DataModel())
                {
                    //Si el estado es 0 (TODO) mostrar los valores unicamente por rango de fecha 
                    if (pEstadoFactura > 0)
                    {
                        ctx.Configuration.LazyLoadingEnabled = false;
                        lista = ctx.FacturaEncabezado.Where(x => x.EstadoFactura == (TypeEstadoFactura)pEstadoFactura && x.FechaFacturacion <= FechaFinal && x.FechaFacturacion >= FechaInicial).ToList().OrderByDescending(x => x.IdFactura);
                    }
                    else
                    {
                        //Obtiene los resultados por TODOS los PARAMETROS expuestos
                        ctx.Configuration.LazyLoadingEnabled = false;
                        lista = ctx.FacturaEncabezado.Where(x => x.FechaFacturacion <= FechaFinal && x.FechaFacturacion >= FechaInicial).ToList().OrderByDescending(x => x.IdFactura);
                    }
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

        public IEnumerable<FacturaEncabezado> GetFacturaEncabezadoPendientes()
        {
            try
            {
                IEnumerable<FacturaEncabezado> lista = null;
                using (DataModel ctx = new DataModel())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    //lista = ctx.FacturaEncabezado.Where(x=>x.EstadoFactura ==TypeEstadoFactura.PENDIENTE).ToList();
                    lista = ctx.FacturaEncabezado.Where(x => x.EstadoFactura == TypeEstadoFactura.PENDIENTE).ToList().OrderByDescending(x => x.IdFactura);
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

        public FacturaEncabezado SaveFacturaEncabezado(FacturaEncabezado facturaEncabezado)
        {
            int retorno = 0;
            String sql = "";
            FacturaEncabezado oFacturaEncabezado = null;
            try
            {
                using (DataModel ctx = new DataModel())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oFacturaEncabezado = GetFacturaEncabezadoById(facturaEncabezado.IdFactura);
                    if (oFacturaEncabezado == null)
                    {
                        //Se consulta la secuencia siguiente
                        sql = string.Format("SELECT NEXT VALUE FOR SequenceNoFactura;");
                        int numero = ctx.Database.SqlQuery<int>(sql).FirstOrDefault();
                        if (numero != 0)
                        {
                            facturaEncabezado.IdFactura = numero;
                            facturaEncabezado.EstadoFactura = TypeEstadoFactura.PENDIENTE;
                            ctx.FacturaEncabezado.Add(facturaEncabezado);
                        }
                    }
                    else
                    {
                        ctx.Entry(facturaEncabezado).State = EntityState.Modified;
                    }
                    retorno = ctx.SaveChanges();
                }

                if (retorno >= 0)
                    oFacturaEncabezado = GetFacturaEncabezadoById(facturaEncabezado.IdFactura);

                return oFacturaEncabezado;
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
      
        // *******************    Detalle   ***********************//

        public void DeleteFacturaDetalle(int idFactura, int secuencia)
        {
            int returno;
            Producto oProducto = new Producto();
            FacturaDetalle oFacturaDetalle = new FacturaDetalle();
            try
            {
                using (DataModel ctx = new DataModel())
                {
                    //Traemos TODA la informacion de la factura detalle antes de ser eliminada
                    oFacturaDetalle = ctx.FacturaDetalle.Where(x => x.Secuencia == secuencia && 
                                                              x.IdFactura== idFactura).FirstOrDefault();
                    //Se agrega al inventario
                    oProducto = ctx.Producto.Where(x => x.IdProducto == oFacturaDetalle.IdProducto).FirstOrDefault();
                    oProducto.Cantidad = oProducto.Cantidad + oFacturaDetalle.Cantidad;
                    


                    //Se elimina el registro
                    ctx.Entry(oFacturaDetalle).State = EntityState.Deleted;
                    returno = ctx.SaveChanges();

                    ctx.Entry(oProducto).State = EntityState.Modified;
                    ctx.SaveChanges();
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

        public IEnumerable<FacturaDetalle> GetFacturaDetalle()
        {
            try
            {
                IEnumerable<FacturaDetalle> lista = null;
                using (DataModel ctx = new DataModel())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    lista = ctx.FacturaDetalle.ToList().OrderByDescending(x => x.Secuencia);
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

        
        public IEnumerable<FacturaDetalle> GetFacturaDetalle(int id)
        {
            try
            {
                IEnumerable<FacturaDetalle> lista = null;
                using (DataModel ctx = new DataModel())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    lista = ctx.FacturaDetalle.Where(x => x.IdFactura == id).ToList().OrderByDescending(x => x.Secuencia);
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

        public FacturaDetalle GetFacturaDetalleById(int idFactura, int  secuencia)
        {
            FacturaDetalle facturaDetalle = null;
            try
            {
                using (DataModel ctx = new DataModel())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;                    
                    facturaDetalle = ctx.FacturaDetalle.
                              Where(p => p.IdFactura == idFactura &&  p.Secuencia == secuencia).
                              FirstOrDefault<FacturaDetalle>();
                }

                return facturaDetalle;
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

      
        public FacturaDetalle SaveFacturaDetalle(FacturaDetalle facturaDetalle)
        {
            int retorno = 0;
            FacturaDetalle oFacturaDetalle = null;
            Producto oProducto = null;
            try
            {
                using (DataModel ctx = new DataModel())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oFacturaDetalle = GetFacturaDetalleById(facturaDetalle.IdFactura,facturaDetalle.Secuencia);
                    if (oFacturaDetalle == null)
                    {
                        ctx.FacturaDetalle.Add(facturaDetalle);
                        //Se rebaja de Inventario del Producto
                        oProducto = ctx.Producto.Where(x => x.IdProducto == facturaDetalle.IdProducto).FirstOrDefault();
                        oProducto.Cantidad = oProducto.Cantidad - facturaDetalle.Cantidad;
                        ctx.Entry(oProducto).State = EntityState.Modified;
                    }
                    else
                    {
                        ctx.Entry(facturaDetalle).State = EntityState.Modified;
                    }
                    retorno = ctx.SaveChanges();
                }

                if (retorno >= 0)
                    oFacturaDetalle = GetFacturaDetalleById(facturaDetalle.IdFactura, facturaDetalle.Secuencia);

                return oFacturaDetalle;
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
