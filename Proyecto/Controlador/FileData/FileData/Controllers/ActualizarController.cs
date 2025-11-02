using Microsoft.AspNetCore.Mvc;
using FileData.Models;
using System.Linq;

namespace FileData.Controllers
{
    public class ActualizarController : Controller
    {
        private readonly BdDataFileContext _context;

        public ActualizarController(BdDataFileContext context)
        {
            _context = context;
        }

        // GET: /Actualizar/ActualizarEmpleado?id=1  (si lo necesitas, lo dejamos)
        [HttpGet]
        public IActionResult ActualizarEmpleado(int id)
        {
            var empleado = _context.Empleados.FirstOrDefault(e => e.IdEmpleado == id);
            if (empleado == null)
                return NotFound();

            return View(empleado);
        }

        // POST tradicional (si lo usas en otra parte, lo dejamos)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ActualizarEmpleado(Empleado model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var empleado = _context.Empleados.FirstOrDefault(e => e.IdEmpleado == model.IdEmpleado);
            if (empleado == null)
                return NotFound();

            empleado.Nombre = model.Nombre;
            empleado.Apellidos = model.Apellidos;
            empleado.FechaNacimiento = model.FechaNacimiento;
            empleado.Edad = model.Edad;
            empleado.EstadoCivil = model.EstadoCivil;
            empleado.Email = model.Email;
            empleado.Telefono = model.Telefono;
            empleado.Dni = model.Dni;

            _context.Update(empleado);
            _context.SaveChanges();

            TempData["MensajeExito"] = "Empleado actualizado correctamente.";
            return RedirectToAction("ConsultarEmpleado", "Home");
        }

        // NUEVO: POST AJAX que recibe formulario y responde JSON
        // No usamos [ValidateAntiForgeryToken] para simplificar el flujo AJAX;
        // si quieres mayor seguridad, añadimos token y validación.
        [HttpPost]
        public JsonResult ActualizarEmpleadoAjax([FromForm] Empleado model)
        {
            if (model == null || model.IdEmpleado == 0)
            {
                return Json(new { success = false, message = "Datos inválidos." });
            }

            var empleado = _context.Empleados.FirstOrDefault(e => e.IdEmpleado == model.IdEmpleado);
            if (empleado == null)
            {
                return Json(new { success = false, message = "Empleado no encontrado." });
            }

            // Actualizar campos
            empleado.Nombre = model.Nombre;
            empleado.Apellidos = model.Apellidos;
            empleado.FechaNacimiento = model.FechaNacimiento;
            empleado.Edad = model.Edad;
            empleado.EstadoCivil = model.EstadoCivil;
            empleado.Email = model.Email;
            empleado.Telefono = model.Telefono;
            empleado.Dni = model.Dni;

            try
            {
                _context.Update(empleado);
                _context.SaveChanges();
                return Json(new { success = true, message = "Empleado actualizado correctamente." });
            }
            catch (System.Exception ex)
            {
                // opcional: loggear ex
                return Json(new { success = false, message = "Error al guardar cambios: " + ex.Message });
            }
        }
    }
}
