using Infraestructure.Models;
using Infraestructure.Models.Catalogo;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{ 
    public class ServiceTipoEvento : IServiceTipoEvento
    {
         
        public void DeleteTipoEvento(int id)
        {
            IRepositoryTipoEvento repository = new RepositoryTipoEvento();

            // Business rules
            if (id >= 0 && id <= 4)
            {
                throw new Exception($"El tipo de evento {id} NO se puede borrar");
            }


            repository.DeleteTipoEvento(id);
        }

        public IEnumerable<TipoEvento> GetTipoEvento()
        {
            IRepositoryTipoEvento repository = new RepositoryTipoEvento();
            return repository.GetTipoEvento();
        }

        public TipoEvento GetTipoEventoByID(int id)
        { 
            RepositoryTipoEvento repository = new RepositoryTipoEvento();
            return repository.GetTipoEventoByID(id);
        }


        public TipoEvento Save(TipoEvento bodega)
        {
            RepositoryTipoEvento repository = new RepositoryTipoEvento();
            
            // Business rules
            if (bodega.DescripcionTipo.Contains("{~}")) {
                throw new Exception("No se permiten estos caracteres ({}) en la descripcion");
            }

            
            return repository.Save(bodega);
        }

        public bool ActivateDeactivate(int id)
        {
            RepositoryTipoEvento repository = new RepositoryTipoEvento();
            return repository.ActivateDeactivate(id);
        }
    }

}