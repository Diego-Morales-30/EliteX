using Microsoft.AspNetCore.Mvc;
using FileData.Models;
using System.Linq;
using System.Text;

namespace FileData.Controllers
{
    public class ConsultarController : Controller
    {
        private readonly BdDataFileContext _context;

        public ConsultarController(BdDataFileContext context)
        {
            _context = context;
        }

        // Vista inicial de consultar empleado
        [HttpGet]
        public IActionResult ConsultarEmpleado()
        {
            return PartialView("~/Views/Home/ConsultarEmpleado.cshtml", Enumerable.Empty<Empleado>());
        }

        // Búsqueda de empleado por nombre completo o ID
        [HttpPost]
        public IActionResult BuscarEmpleado(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return Content("<p>Ingrese un filtro para buscar empleados.</p>");

            var empleados = _context.Empleados
                .Where(e => e.IdEmpleado.ToString() == filtro ||
                            (e.Nombre + " " + e.Apellidos).Contains(filtro))
                .ToList();

            if (!empleados.Any())
                return Content("<p>Empleado no encontrado.</p>");

            var sb = new StringBuilder();
            foreach (var e in empleados)
            {
                sb.Append($@"
                    <div>
                        <h3>{e.Nombre} {e.Apellidos}</h3>
                        <p><strong>ID:</strong> {e.IdEmpleado}</p>
                        <p><strong>Fecha Nacimiento:</strong> {e.FechaNacimiento:dd/MM/yyyy}</p>
                        <p><strong>Edad:</strong> {e.Edad}</p>
                        <p><strong>Estado Civil:</strong> {e.EstadoCivil}</p>
                        <p><strong>Email:</strong> {e.Email}</p>
                        <p><strong>Teléfono:</strong> {e.Telefono}</p>
                        <p><strong>DNI:</strong> {e.Dni}</p>
                        <hr/>
                    </div>
                ");
            }

            return Content(sb.ToString(), "text/html");
        }
    }
}
