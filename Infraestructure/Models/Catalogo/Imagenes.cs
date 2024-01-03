using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Models.Catalogo
{
    public class Imagenes
    {
        //public IEnumerable<string> Images { get; set; }
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Localidades { get; set; }
        public string Image { get; set; }
        public string Descripcion { get; set; }
    }

    public partial class SampleData
    {
        public static readonly IEnumerable<Imagenes> images = new[] {
            new Imagenes {
                ID = 1,
                Nombre = "BAD BUNNY - Un Verano Sin Ti - Estadio Nacional",
                Localidades = "DanceFloor - BAC VIP - General",
                Image = "../../Content/images/images/bb-uvst.jpg",
                Descripcion = "BAD BUNNY en Costa Rica con su nuevo disco Un Verano Sin Ti, el evento se realizara el dia 17/09/2022 a las 21:30," + 
                "Ven a disfrutar con el conejo malo y sus grandes exitos."

            },
            new Imagenes {
                ID = 2,
                Nombre = "Pic-Nic Fest - 2022",
                Localidades = "General - VIP - PicNicPASS",
                Image = "../../Content/images/images/pinic foto.png",
                Descripcion="¡El Festival más grande y con mayor contenido de Costa Rica y la región! Este año con nuestra edicion 2022."
            },
            new Imagenes {
                ID = 3,
                Nombre = "Saprissa VS Alajuela (IDA)",
                Localidades = "Palco sur - Sombra - VIP",
                Image = "../../Content/images/images/s.png",
                Descripcion="Partido de IDA, torneo de invierno, Ricardo Saprissa."
            },
            new Imagenes {
                ID = 4,
                Nombre = "Motocross - Rancho Centenario",
                Localidades = "General - VIP",
                Image = "../../Content/images/images/motocross.png",
                Descripcion="Evento de Motocross con los mejores pilotos del sector, habran ricas comidas y juegos colectivos."
            },
        };
    }
}
