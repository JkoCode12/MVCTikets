using Infraestructure.Models;
using Infraestructure.Models.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
    public  interface IRepositoryTipoEvento
    {        
        IEnumerable<TipoEvento> GetTipoEvento();
        TipoEvento GetTipoEventoByID(int id);
        void DeleteTipoEvento(int id);
        TipoEvento Save(TipoEvento tipo);
        bool ActivateDeactivate(int id);
    }
}
