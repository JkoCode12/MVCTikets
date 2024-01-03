using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Models.Catalogo
{
    [Table("ROL")]
    public partial class Rol
    {


        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Rol")]
        [Column("IdRol")]
        public int IdRol { set; get; }

        [Required(ErrorMessage = "{0} es requerido")]
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
        [Display(Name = "Descripción")]
        [Column("Descripcion")]
        public string Descripcion { set; get; }
 
    }
}