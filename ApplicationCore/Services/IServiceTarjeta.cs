using Infraestructure.Models;
using Infraestructure.Models.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{

    public interface IServiceTarjeta
    { 
        IEnumerable<Tarjeta> GetTarjeta();
        Tarjeta GetTarjetaByID(int id);
        void DeleteTarjeta(int id);
        Tarjeta Save(Tarjeta tarjeta);
    }
}
