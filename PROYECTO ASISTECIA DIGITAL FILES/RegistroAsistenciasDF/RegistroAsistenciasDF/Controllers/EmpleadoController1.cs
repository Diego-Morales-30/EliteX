using Microsoft.AspNetCore.Mvc;
using RegistroAsistenciasDF.Models;

namespace RegistroAsistenciasDF.Controllers
{
    public class EmpleadoController1 : Controller
    {
        public IActionResult Index()
        {
            List<Empleado> listaProductos = new List<Empleado>();

            using (TiendaVirtualDBContext BD = new TiendaVirtualDBContext())
            {
                listaProductos = (from p in BD.Producto select p).ToList();
            }

            return View(listaProductos);
        }
    }
}
