using FileData.Models;
using Microsoft.AspNetCore.Mvc;

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
                UsuarioRegistro = 0,
                FechaRegistro = DateTime.Now
            };

            // 🔹 Validación: La fecha no puede ser anterior a hoy
            if (horario.FechaInicio < DateOnly.FromDateTime(DateTime.Today))
            {
                return BadRequest(new { mensaje = "❌ La fecha de inicio no puede ser anterior a la fecha actual." });
            }

            // 🔹 Validación: Si hay hora de salida, debe ser 8 horas después de la entrada
            if (horario.HoraSalida.HasValue)
            {
                var diferencia = horario.HoraSalida.Value.ToTimeSpan() - horario.HoraEntrada.ToTimeSpan();

                if (diferencia.TotalHours != 8)
                {
                    return BadRequest(new { mensaje = "❌ El horario debe tener exactamente 8 horas de duración." });
                }

                if (horario.HoraSalida <= horario.HoraEntrada)
                {
                    return BadRequest(new { mensaje = "❌ La hora de salida debe ser posterior a la hora de entrada." });
                }
            }

            // 🔹 Validación: Evitar duplicados para el mismo día
            bool yaTieneHorario = _context.Horarios.Any(h =>
                h.IdEmpleado == horario.IdEmpleado && h.FechaInicio == horario.FechaInicio);

            if (yaTieneHorario)
            {
                return BadRequest(new { mensaje = "⚠️ Este empleado ya tiene un horario asignado para esa fecha." });
            }

            // ✅ Guardar si pasa todas las validaciones
            _context.Horarios.Add(horario);
            _context.SaveChanges();

            return Ok(new { mensaje = "Horario registrado correctamente." });
        }




    }
}

