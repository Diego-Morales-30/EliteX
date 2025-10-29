using Microsoft.AspNetCore.Mvc;
using FileData.Models;
using Microsoft.EntityFrameworkCore;

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
        [ValidateAntiForgeryToken]
        public IActionResult RegistrarEmpleado(Empleado empleado)
        {
            if (!ModelState.IsValid)
            {
                // ❌ Si hay errores de validación del modelo
                return View(empleado);
            }

            // ⚠️ Verificar si ya existe empleado con mismo DNI o correo
            var existe = _context.Empleados
                .Any(e => e.Dni == empleado.Dni || e.Email == empleado.Email);

            if (existe)
            {
                ViewBag.Error = "El usuario ya se encuentra registrado.";
                return View(empleado);
            }

            try
            {
                // EF genera el Id automáticamente si es identidad
                _context.Empleados.Add(empleado);
                _context.SaveChanges();

                ViewBag.Exito = "Empleado registrado correctamente.";
                ModelState.Clear(); // Limpia el formulario

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error al registrar empleado: {ex.Message}";
                return View(empleado);
            }
        }
    }
}


