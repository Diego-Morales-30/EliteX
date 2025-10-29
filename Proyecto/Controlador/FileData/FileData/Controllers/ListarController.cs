using Microsoft.AspNetCore.Mvc;
using FileData.Models;
using System.Linq;

namespace FileData.Controllers
{
        public class ListarController : Controller
        {
        private readonly BdDataFileContext _context;

        public ListarController(BdDataFileContext context)
        {
            _context = context;
        }

        public IActionResult ListarEmpleado()
        {
            // Obtiene todos los empleados desde la base de datos
            var empleados = _context.Empleados.ToList();

            // Devuelve la vista parcial con la lista
            return PartialView("~/Views/Home/ListarEmpleado.cshtml", empleados);
        }
    }

}