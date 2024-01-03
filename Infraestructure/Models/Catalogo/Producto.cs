using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Models.Catalogo
{

    [Table("PRODUCTO")]
    public class Producto
    {

        [Key,DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Código de Evento")]
        [Column("IdProducto")]
        [Required(ErrorMessage = "{0} es requerido")]
        public virtual int IdProducto { set; get; }

       
        [Display(Name = "Tipo de evento")]
        [Column("IdTipo")]
        public virtual int IdTipo { set; get; }

        [ForeignKey("IdTipo")]
        public virtual TipoEvento Tipo { set; get; }


        [Required(ErrorMessage = "{0} es requerido")]
        [MaxLength(100, ErrorMessage = "Máximo 50 caracteres")]
        [Display(Name = "Descripción Evento")]
        [Column("DescripcionElectronico")]
        public virtual string DescripcionElectronico { get; set; }

        [Display(Name = "Precio Colones")]
        [Column("CostoColones")]
        public Decimal CostoDolar { get; set; }
        //public virtual decimal? CostoDolar { get; set; }

        [Display(Name = "Precio")]
        [Column("Precio")]
        [Required(ErrorMessage = "{0} es requerido")]
        public virtual Decimal Precio { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
        [Display(Name = "Sector")]
        [Column("Sector")]
        public virtual string Sector { get; set; }


        [Display(Name = "Cantidad")]
        [Column("Cantidad")]
        [Required(ErrorMessage = "{0} es requerido")]
        public virtual int Cantidad { get; set; }
        
        [Display(Name = "Fecha programada para el evento")]
        [Column("FechaInclusion")]
        [Required(ErrorMessage = "{0} es requerido")]
        public virtual DateTime FechaInclusion { get; set; }

        [Display(Name = "Estado")]
        [Column("Estado")]
        [EnumDataType(typeof(TypeEstado))]
        public TypeEstado Estado { set; get; }

        [NotMapped]
        public String MostrarProducto
        {
            get
            {
                Repository.IRepositoryTipoEvento repositoryTipo = new Repository.RepositoryTipoEvento();

                return String.Format("{0} | {1} | {2} |", repositoryTipo.GetTipoEventoByID(this.IdTipo).DescripcionTipo, this.DescripcionElectronico, this.FechaInclusion);
            }
        }

        [NotMapped]
        public String MostrarColones
        {
            get
            {

                return String.Format("{0}", Precio*700);
            }
        }

        public String MostrarProducto2
        {
            get
            {

                return String.Format("{0} | {1} |", this.IdTipo, this.DescripcionElectronico);
            }
        }


    }
}
