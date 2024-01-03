using ApplicationCore.Utils;
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
    public class ServiceEmpleado : IServiceEmpleado
    {
        public void DeleteEmpleado(string id)
        {
            IRepositoryEmpleado repository = new RepositoryEmpleado();
            repository.DeleteEmpleado(id);
        }

        public IEnumerable<Empleado> GetEmpleado()
        {
            IRepositoryEmpleado repository = new RepositoryEmpleado();
            return repository.GetEmpleado();
        }

        public Empleado GetEmpleadoByID(string id)
        {
            IRepositoryEmpleado repository = new RepositoryEmpleado();
            return repository.GetEmpleadoByID(id);
        }

        public Empleado Save(Empleado empleado)
        {
            IRepositoryEmpleado repository = new RepositoryEmpleado();
            return repository.Save(empleado);
        }

        public Empleado GetEmpleado(string id, string password) {
            IRepositoryEmpleado repository = new RepositoryEmpleado();

            // Se encripta el valor que viene y se compara con el valor encriptado en al BD.
            //string crytpPasswd = Cryptography.EncrypthAES(password); 

            return repository.GetEmpleado(  id, password);

        }

    }
}
