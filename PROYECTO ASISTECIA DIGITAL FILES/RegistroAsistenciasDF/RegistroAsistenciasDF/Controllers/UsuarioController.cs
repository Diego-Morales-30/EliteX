using Microsoft.AspNetCore.Mvc;
using RegistroAsistenciasDF.Models;

namespace RegistroAsistenciasDF.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult MostrarUsuario(int Id)
        {
            Usuario usuario = new Usuario();
            using (RegistroDfContext BD = new RegistroDfContext())
            {

                var listarUsuario = (from c in BD.Usuarios
                                     where c.IdUsuario == Id
                                     select new Usuario
                                     {
                                         IdUsuario = c.IdUsuario,
                                         Username = c.Username,
                                         PasswordHash = c.PasswordHash

                                     }).FirstOrDefault();

                if (usuario == null)
                    return Content("No se encontró el usuario con ese ID.");

                return View(listarUsuario);

            }
        }
    }
}

