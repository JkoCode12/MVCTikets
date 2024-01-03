using Infraestructure.Models;
using Infraestructure.Models.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IServiceEmpleado
    {
        IEnumerable<Empleado> GetEmpleado();
        Empleado GetEmpleadoByID(string id);

        Empleado GetEmpleado(string id,string password);
        void DeleteEmpleado(string id);
        Empleado Save(Empleado empleado);
    }
}
