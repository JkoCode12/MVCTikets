using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Models.Catalogo
{
    [Table("Tarjeta")]
    public class Tarjeta
    {
        #region Atributos
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Código")]
        [Required(ErrorMessage = "{0} es requerido")]
        [Column("IdTarjeta")]
        public int IdTarjeta { set; get; }

        [Required(ErrorMessage = "{0} es requerido")]
        [MaxLength(50, ErrorMessage = "Máximo {1} caracteres")]
        [Display(Name = "Nombre de la tarjeta")]
        [Column("DescripcionTarjeta")]
        public string DescripcionTarjeta { get; set; }

        [Display(Name = "Estado")]
        [Column("Estado")]
        [EnumDataType(typeof(TypeEstadoTarjeta))]
        public TypeEstadoTarjeta Estado { set; get; }
        #endregion

        #region Constructor
        public Tarjeta() { 
        }
        #endregion

    }
}
