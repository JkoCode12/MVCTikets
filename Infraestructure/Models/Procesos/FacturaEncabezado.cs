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
    [Table("FacturaEncabezado")]
    public class FacturaEncabezado
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required(ErrorMessage = "{0} es requerido")]
        [Display(Name = "Número Factura")]
        [Column("IdFactura")]
        public int IdFactura { get; set; }

        [Display(Name = "Tarjeta")]
        [Column("IdTarjeta")]
        public int IdTarjeta { get; set; }

        [Display(Name = "Número de Tarjeta")]
        [MaxLength(17, ErrorMessage = "Máximo {1} caracteres")]
        [RegularExpression("^[0-9]{1,17}?$", ErrorMessage = "Sólo se aceptan numéros")]
        [Column("TarjetaNumero")]
        public string TarjetaNumero { get; set; }

        [Display(Name = "Cliente")]
        [Column("IdCliente")]
        public string IdCliente { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de facturación")]
        [Column("FechaFacturacion")]
        public DateTime? FechaFacturacion { get; set; }

        [Display(Name = "Estado")]
        [Column("EstadoFactura")]
        [EnumDataType(typeof(TypeEstadoFactura))]
        public TypeEstadoFactura EstadoFactura { set; get; }

    }
}
