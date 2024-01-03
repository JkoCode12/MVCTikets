using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Models.Catalogo
{
    [Table("EMPLEADO")]
    public class Empleado
    {      

        [Key]
        [Required(ErrorMessage = "{0} es requerido")]
        [MaxLength(20, ErrorMessage = "Máximo {1} caracteres")]
        [Display(Name = "Código")]
        [Column("Login")]
        public string Login { get; set; }

        [Display(Name = "Rol")]
        [Column("IdRol")]
        [EnumDataType(typeof(TypeRoles))]
        public TypeRoles IdRol { get; set; }

        [Display(Name = "Estado")]
        [Column("Estado")]
        [EnumDataType(typeof(TypeEstado))]
        public TypeEstado Estado { set; get; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Cedula")]
        [Column("Cedula")]
        public int Cedula { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [Display(Name = "Contraseña")]
        [Column("Password")]  //No es requerido si el nombre es igual al de la base de datos
        public string Password { get; set; }

        [Display(Name = "Nombre")]
        [RegularExpression(@"^[A-Z a-z0-9ÑñáéíóúÁÉÍÓÚ\\-\\_]+$", ErrorMessage = "Sólo se aceptan letras y números")]
        [MaxLength(20, ErrorMessage = "Máximo {1} caracteres")]
        [Column("Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Apellidos")]
        [RegularExpression(@"^[A-Z a-z0-9ÑñáéíóúÁÉÍÓÚ\\-\\_]+$", ErrorMessage = "Sólo se aceptan letras y números")]
        [MaxLength(40, ErrorMessage = "Máximo {1} caracteres")]
        [Column("Apellidos")]
        public string Apellidos { get; set; }
         

        [NotMapped]
        public String MostrarEmpleados
        {
            get
            {
                return String.Format("{0}-{1} {2}", this.Login, this.Nombre, this.Apellidos);
            }
        }
       
    }
}
