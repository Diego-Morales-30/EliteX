using Microsoft.AspNetCore.Mvc;
using FileData.Models;
using System.Linq;

namespace FileData.Controllers
{
    public class EmpleadoController : Controller
    {
        private readonly BdDataFileContext _context;

        public EmpleadoController(BdDataFileContext context)
        {
            _context = context;
        }

        // GET: /Empleado/RegistrarEmpleado
        [HttpGet]
        public IActionResult RegistrarEmpleado()
        {
            return View();
        }

        // POST: /Empleado/RegistrarEmpleado
        [HttpPost]
        public JsonResult RegistrarEmpleado(Empleado empleado)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Datos inválidos." });
            }

            var existe = _context.Empleados
                .Any(e => e.Dni == empleado.Dni || e.Email == empleado.Email);

            if (existe)
            {
                return Json(new { success = false, message = "El empleado ya está registrado." });
            }

            try
            {
                _context.Empleados.Add(empleado);
                _context.SaveChanges();

                return Json(new { success = true, message = "Empleado registrado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }
    }
}





