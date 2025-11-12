using Microsoft.AspNetCore.Mvc;
using FileData.Models;
using Microsoft.EntityFrameworkCore;

namespace FileData.Controllers
{
    public class EliminaEmpleadoController : Controller
    {
        private readonly BdDataFileContext _context;

        public EliminaEmpleadoController(BdDataFileContext context)
        {
            _context = context;
        }

        // GET: Vista principal
        [HttpGet]
        public IActionResult EliminarEmpleado()
        {
            return View("~/Views/Eliminar/EliminarEmpleado.cshtml");
        }

        // GET: Buscar empleado por DNI
        [HttpGet]
        public IActionResult BuscarEmpleado(string dni)
        {
            var empleado = _context.Empleados
                .Include(e => e.Usuario)
                .Include(e => e.Horarios)
                .FirstOrDefault(e => e.Dni == dni);

            if (empleado == null)
                return Json(new { success = false, message = "No se encontró un empleado con ese DNI." });

            return Json(new
            {
                success = true,
                data = new
                {
                    empleado.IdEmpleado,
                    empleado.Nombre,
                    empleado.Apellidos,
                    empleado.Email,
                    empleado.Telefono,
                    empleado.Dni,
                    empleado.Edad,
                    empleado.EstadoCivil,
                    usuario = empleado.Usuario?.Usuario1 ?? "(Sin usuario asignado)"
                }
            });
        }

        // POST: Eliminar empleado (en cascada)
        [HttpPost]
        public IActionResult EliminarEmpleadoConfirmado(int id)
        {
            var empleado = _context.Empleados
                .Include(e => e.Usuario)
                .Include(e => e.Horarios)
                .FirstOrDefault(e => e.IdEmpleado == id);

            if (empleado == null)
                return Json(new { success = false, message = "Empleado no encontrado." });

            // Eliminar horarios asociados directamente al empleado
            if (empleado.Horarios.Any())
                _context.Horarios.RemoveRange(empleado.Horarios);

            // Eliminar horarios registrados por el usuario (UsuarioRegistro)
            if (empleado.Usuario != null)
            {
                var horariosUsuario = _context.Horarios
                    .Where(h => h.UsuarioRegistro == empleado.Usuario.IdUsuario)
                    .ToList();

                if (horariosUsuario.Any())
                    _context.Horarios.RemoveRange(horariosUsuario);
            }

            // Eliminar usuario del empleado
            if (empleado.Usuario != null)
                _context.Usuarios.Remove(empleado.Usuario);

            // Eliminar empleado
            _context.Empleados.Remove(empleado);
            _context.SaveChanges();

            return Json(new { success = true, message = "Empleado eliminado correctamente." });
        }
    }
}
