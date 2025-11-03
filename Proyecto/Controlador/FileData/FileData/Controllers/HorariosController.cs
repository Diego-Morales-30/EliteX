using Microsoft.AspNetCore.Mvc;
using FileData.Models;
using System.Linq;

namespace FileData.Controllers
{
    public class HorariosController : Controller
    {
        private readonly BdDataFileContext _context;

        public HorariosController(BdDataFileContext context)
        {
            _context = context;
        }

        // 🔹 GET: Mostrar formulario
        [HttpGet]
        public IActionResult RegistrarHorario()
        {
            ViewBag.Empleados = _context.Empleados.ToList();

            return PartialView("RegistrarHorario");
        }

        // 🔹 POST: Guardar datos
        [HttpPost]
        public IActionResult RegistrarHorario(IFormCollection form)
        {

            var horario = new Horario
            {
                IdEmpleado = int.Parse(form["IdEmpleado"]),
                FechaInicio = DateOnly.Parse(form["FechaInicio"]),
                HoraEntrada = TimeOnly.Parse(form["HoraEntrada"]),
                HoraSalida = string.IsNullOrEmpty(form["HoraSalida"])
                    ? null
                    : TimeOnly.Parse(form["HoraSalida"]),
                Observaciones = form["Observaciones"],
                UsuarioRegistro = 1,
                FechaRegistro = DateTime.Now
            };

            _context.Horarios.Add(horario);
            _context.SaveChanges();

            return Ok(new { mensaje = "Horario registrado correctamente." });
        }

    }
}

