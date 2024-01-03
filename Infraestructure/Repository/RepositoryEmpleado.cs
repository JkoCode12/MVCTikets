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
    public class RepositoryEmpleado : IRepositoryEmpleado
    {
        public void DeleteEmpleado(string id)
        {
            int returno;
            try
            {

                using (DataModel ctx = new DataModel())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    Empleado empleado = new Empleado()
                    {
                        Login = id
                    };
                    ctx.Entry(empleado).State = EntityState.Deleted;
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

        public Empleado GetEmpleadoByID(string id)
        {
            Empleado empleado = null;
            try
            {

                using (DataModel ctx = new DataModel())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    empleado = ctx.Empleado.
                              Where(p => p.Login == id).
                              FirstOrDefault<Empleado>();
                }

                return empleado;
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

        public IEnumerable<Empleado> GetEmpleado()
        {
            try
            {

                IEnumerable<Empleado> lista = null;
                using (DataModel ctx = new DataModel())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    lista = ctx.Empleado.Include("Rol").ToList<Empleado>();
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

        public Empleado Save(Empleado empleado)
        {
            int retorno = 0;
            Empleado oEmpleado = null;
            try
            {
                using (DataModel ctx = new DataModel())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oEmpleado = GetEmpleadoByID(empleado.Login);
                    if (oEmpleado == null)
                    {
                        ctx.Empleado.Add(empleado);
                    }
                    else
                    {
                        ctx.Entry(empleado).State = EntityState.Modified;
                    }
                    retorno = ctx.SaveChanges();
                }

                if (retorno >= 0)
                    oEmpleado = GetEmpleadoByID(empleado.Login);

                return oEmpleado;
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



        public Empleado GetEmpleado(string id, string password)
        {

            Empleado oEmpleado = null;
            try
            {

                using (DataModel ctx = new DataModel())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oEmpleado = ctx.Empleado.
                                 Where(p => p.Login.Equals(id) && p.Password == password).
                                 FirstOrDefault<Empleado>();
                }

                if (oEmpleado != null)
                    oEmpleado = GetEmpleadoByID(id);

                return oEmpleado;
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

        public string[] GetRolesForUser(string username)
        {

            List<Rol> lista = new List<Rol>();
            string[] roles = null;
            String sql = "";
            List<string> userRoles = new List<string>();
            try
            {
                using (DataModel db = new DataModel())
                {
                    sql = string.Format("select r.* from Rol r, Empleado u where u.IdRol=r.IdRol and u.Login='{0}'", username.Trim());
                    lista = db.Rol.SqlQuery(sql).ToList<Rol>();
                    userRoles = lista.Select(r => r.Descripcion).ToList();
                }

                if (lista != null)
                {
                    roles = userRoles.ToArray();
                }
                else
                {
                    roles = new string[] { "Ninguno", "Ninguno" };
                }
                return roles;
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
