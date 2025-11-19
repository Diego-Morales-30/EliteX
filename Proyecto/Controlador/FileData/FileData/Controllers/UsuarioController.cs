using FileData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FileData.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly BdDataFileContext _context;

        public UsuarioController(BdDataFileContext context)
        {
            _context = context;
        }

        // GET: /Usuario/Eliminar
        // Muestra la lista de usuarios con botón "Eliminar usuario"
        public IActionResult Eliminar()
        {
            var usuarios = _context.Usuarios
                .Include(u => u.IdEmpleadoNavigation) // para mostrar nombre del empleado
                .ToList();

            return View(usuarios); // Va a buscar Views/Usuario/Eliminar.cshtml
        }

        // POST: /Usuario/EliminarConfirmado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarConfirmado(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.IdUsuario == id);
            if (usuario == null)
            {
                return Json(new { success = false, message = "El usuario ya no existe." });
            }

            // Solo se elimina el USUARIO, no el empleado
            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();

            return Json(new { success = true, message = "Usuario eliminado correctamente." });
        }
    }
}
