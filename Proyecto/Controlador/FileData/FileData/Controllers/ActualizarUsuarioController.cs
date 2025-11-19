using FileData.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FileData.Controllers
{
    public class ActualizarUsuarioController : Controller
    {
        private readonly BdDataFileContext _context;

        public ActualizarUsuarioController(BdDataFileContext context)
        {
            _context = context;
        }

        // ================================
        //   GET — Cargar usuario a editar
        // ================================
        [HttpGet]
        public IActionResult ActualizarUsuario(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.IdUsuario == id);
            if (usuario == null)
                return NotFound();

            return View(usuario);
        }


        // ================================
        //   POST tradicional (NO AJAX)
        // ================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ActualizarUsuario(Usuario model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = _context.Usuarios.FirstOrDefault(u => u.IdUsuario == model.IdUsuario);
            if (usuario == null)
                return NotFound();

            // Solo actualizamos campos editables:
            usuario.Usuario1 = model.Usuario1;
            usuario.Contrasena = model.Contrasena;
            usuario.Cargo = model.Cargo;

            // IdEmpleado NO se cambia porque así lo pediste
            // usuario.IdEmpleado = usuario.IdEmpleado;

            try
            {
                _context.Update(usuario);
                _context.SaveChanges();

                TempData["MensajeExito"] = "Usuario actualizado correctamente.";
                return RedirectToAction("ConsultarUsuario", "Home");
            }
            catch (System.Exception ex)
            {
                TempData["MensajeError"] = "Error al guardar cambios: " + ex.Message;
                return View(model);
            }
        }


        // ================================
        //   POST AJAX — Respuesta JSON
        // ================================
        [HttpPost]
        public JsonResult ActualizarUsuarioAjax([FromForm] Usuario model)
        {
            if (model == null || model.IdUsuario == 0)
            {
                return Json(new { success = false, message = "Datos inválidos." });
            }

            var usuario = _context.Usuarios.FirstOrDefault(u => u.IdUsuario == model.IdUsuario);
            if (usuario == null)
            {
                return Json(new { success = false, message = "Usuario no encontrado." });
            }

            // Actualizamos solamente: Usuario, Contraseña, Cargo
            usuario.Usuario1 = model.Usuario1;
            usuario.Contrasena = model.Contrasena;
            usuario.Cargo = model.Cargo;

            // IdEmpleado NO se modifica
            // usuario.IdEmpleado = usuario.IdEmpleado;

            try
            {
                _context.Update(usuario);
                _context.SaveChanges();

                return Json(new { success = true, message = "Usuario actualizado correctamente." });
            }
            catch (System.Exception ex)
            {
                return Json(new { success = false, message = "Error al guardar cambios: " + ex.Message });
            }
        }
    }
}
