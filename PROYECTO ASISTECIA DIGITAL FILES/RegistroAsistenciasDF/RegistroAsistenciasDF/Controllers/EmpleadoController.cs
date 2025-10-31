using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using RegistroAsistenciasDF.Models;
using System.Linq;

namespace RegistroAsistenciasDF.Controllers
{
    public class EmpleadoController : Controller
    {
        public IActionResult MostrarEmpleado(int Id)
        {
            Empleado empleado = new Empleado();
            using (RegistroDfContext BD = new RegistroDfContext())
            {

                var listaEmpleado = (from c in BD.Empleados
                                     where c.IdEmpleado == Id
                                     select new Empleado
                                     {
                                         IdEmpleado = c.IdEmpleado,
                                         Nombre = c.Nombre,
                                         Apellido = c.Apellido

                                     }).FirstOrDefault();

                if (empleado == null)
                    return Content("No se encontró el empleado con ese ID.");

                return View(listaEmpleado);
                    
            }
        }
    }
}

