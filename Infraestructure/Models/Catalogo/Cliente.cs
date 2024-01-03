using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Models.Catalogo
{
    [Table("CLIENTE")]
    public class Cliente
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Cedula")]
        [Required(ErrorMessage = "{0} es requerido")]
        public string IdCliente { get; set; }
        
        [Required(ErrorMessage = "{0} es requerido")]
        public string Nombre { get; set; }
        
        [Required(ErrorMessage = "{0} es requerido")]
        [Display(Name = "Primer Apellido")]
        public string Apellido1 { get; set; }
        
        [Required(ErrorMessage = "{0} es requerido")]
        [Display(Name = "Segundo Apellido")]
        public string Apellido2 { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        [Display(Name = "Email")]
        [Column("Email")]
        public virtual string Email { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        public string Sexo { get; set; }
        
        
        [Required(ErrorMessage = "{0} es requerido")]
        
        public System.DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(50, ErrorMessage = "Máximo 100 caracteres")]
        [Display(Name = "Nacionalidad")]
        [Column("Nacionalidad")]
        public string Nacionalidad { get; set; }
        
        [NotMapped]
        public String MostrarCliente
        {
            get
            {
                return String.Format("{0}-{1} {2} {3}",this.IdCliente.Trim(), this.Nombre, this.Apellido1, this.Apellido2);
            }
        }

    }
}
