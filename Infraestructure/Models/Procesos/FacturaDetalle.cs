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
    [Table("FacturaDetalle")]
    public class FacturaDetalle
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required(ErrorMessage = "{0} es requerido")]
        [Column(Order = 1)]
        [Display(Name = "No Factura")]        
        public int IdFactura { get; set; }
        [Column(Order = 2)]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Secuencia { get; set; } 
        
        [Display(Name = "Evento")]
        [Column("IdProducto")]
        public int IdProducto { get; set; }

        [Display(Name = "Cantidad")]
        [Column("Cantidad")]
        [Range(1,100, ErrorMessage = "El valor permitido para la {0} debe estar entre {1} y {2}.")]
        public int Cantidad { get; set; }

        [Display(Name = "Precio")]
        [Column("Precio")]
        public decimal Precio { get; set; }

        [Display(Name = "Total por Linea")]
        [Column("TotalLinea")]
        public decimal TotalLinea { get; set; }

        [Display(Name = "Impuesto")]
        [Column("Impuesto")]
        public decimal Impuesto { get; set; }

        [NotMapped]
        [Display(Name = "Cantidad Disponible")]
        public int CantidadDisponible { get; set; }

    }
}
