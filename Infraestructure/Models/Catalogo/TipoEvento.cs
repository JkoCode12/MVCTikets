using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Models.Catalogo
{
    [Table("TIPOEVENTO")]
    public class TipoEvento
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Código")]
        [Column("IdTipo")]
        public virtual int IdTipo{ set; get; }

        [Required(ErrorMessage = "{0} es requerido")]
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
        [Display(Name = "Descripcion del tipo")]
        [Column("DescripcionTipo")]
        public string DescripcionTipo { get; set; }

        [Display(Name = "Estado")]
        [Column("Estado")]
        [EnumDataType(typeof(TypeEstado))]
        public TypeEstado Estado { set; get; }

     

        public virtual ICollection<Producto> Producto { set; get; }   
    }
}
