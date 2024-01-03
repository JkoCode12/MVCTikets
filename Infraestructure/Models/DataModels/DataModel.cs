using Infraestructure.Models.Catalogo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Models.DataModels
{
    public class DataModel : DbContext
    {
        public DataModel() : base("name=DataModelDB")
        {
        }
        #region Seguridad

        public virtual DbSet<Empleado> Empleado { get; set; }
        public virtual DbSet<Rol> Rol { get; set; }
        #endregion

        #region Catalogos
        public virtual DbSet<TipoEvento> TipoEvento { get; set; }
        public virtual DbSet<Producto> Producto { get; set; }
        public virtual DbSet<Tarjeta> Tarjeta { get; set; }
        public virtual DbSet<Cliente> Cliente { get; set; }
        #endregion

        #region Procesos
        public virtual DbSet<FacturaEncabezado> FacturaEncabezado { get; set; }
        public virtual DbSet<FacturaDetalle> FacturaDetalle { get; set; }
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Se debe colocar para eliminar la pluralidad
          
            modelBuilder.Entity<Cliente>().ToTable("Cliente");
            modelBuilder.Entity<TipoEvento>().ToTable("TipoEvento");
            modelBuilder.Entity<Producto>().ToTable("Producto");
            modelBuilder.Entity<FacturaEncabezado>().ToTable("FacturaEncabezado");
            modelBuilder.Entity<FacturaDetalle>().ToTable("FacturaDetalle");

        }
    }
}
