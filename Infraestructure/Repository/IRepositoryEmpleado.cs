using Infraestructure.Models;
using Infraestructure.Models.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
    public interface IRepositoryEmpleado
    {

        IEnumerable<Empleado> GetEmpleado();       
        Empleado GetEmpleadoByID(string id);
        void DeleteEmpleado(string id);
        Empleado Save(Empleado empleado);
        string[] GetRolesForUser(string username);

        Empleado GetEmpleado(string id, string password);
    }
}
