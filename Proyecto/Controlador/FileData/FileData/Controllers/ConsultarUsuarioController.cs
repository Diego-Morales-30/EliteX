using Microsoft.AspNetCore.Mvc;
using FileData.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Net;

namespace FileData.Controllers
{
    public class ConsultarUsuarioController : Controller
    {
        private readonly BdDataFileContext _context;

        public ConsultarUsuarioController(BdDataFileContext context)
        {
            _context = context;
        }

        // GET: carga la vista parcial (sin datos)
        [HttpGet]
        public IActionResult ConsultarUsuario()
        {
            // devolvemos la vista que contiene el buscador (igual patrón que Empleado)
            return PartialView("~/Views/Home/ConsultarUsuario.cshtml", Enumerable.Empty<Usuario>());
        }

        // POST: buscar por id, username o nombre completo del empleado asociado
        [HttpPost]
        public IActionResult BuscarUsuario(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
                return Content("<p>Ingrese un criterio de búsqueda.</p>");

            var usuarios = _context.Usuarios
                .Include(u => u.IdEmpleadoNavigation)
                .Where(u =>
                    u.IdUsuario.ToString() == filtro ||
                    u.Usuario1.Contains(filtro) ||
                    (u.IdEmpleadoNavigation != null &&
                     (u.IdEmpleadoNavigation.Nombre + " " + u.IdEmpleadoNavigation.Apellidos).Contains(filtro))
                )
                .ToList();

            if (!usuarios.Any())
                return Content("<p>No se encontraron usuarios con ese criterio.</p>");

            var sb = new StringBuilder();
            foreach (var u in usuarios)
            {
                var nombreEmp = u.IdEmpleadoNavigation != null
                    ? $"{WebUtility.HtmlEncode(u.IdEmpleadoNavigation.Nombre)} {WebUtility.HtmlEncode(u.IdEmpleadoNavigation.Apellidos)}"
                    : "-";

                var emailEmp = u.IdEmpleadoNavigation?.Email ?? "-";

                sb.Append($@"
                    <div class='usuario-card'>
                        <h3>{WebUtility.HtmlEncode(u.Usuario1)} <small style='color:#9fbce8;'>(ID {u.IdUsuario})</small></h3>
                        <p><strong>Cargo:</strong> {WebUtility.HtmlEncode(u.Cargo ?? "-")}</p>
                        <p><strong>Empleado:</strong> {nombreEmp}</p>
                        <p><strong>Email:</strong> {WebUtility.HtmlEncode(emailEmp)}</p>
                        <p><strong>IdEmpleado:</strong> {(u.IdEmpleado?.ToString() ?? "-")}</p>
                        <hr />
                    </div>");
            }

            return Content(sb.ToString(), "text/html");
        }
    }
}
