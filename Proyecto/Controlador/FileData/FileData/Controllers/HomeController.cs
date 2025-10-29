using FileData.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace FileData.Controllers
{
    public class HomeController : Controller
    {
        private readonly BdDataFileContext _context;

        public HomeController(BdDataFileContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // Muestra el formulario de registro
        [HttpGet]
        public IActionResult RegistrarEmpleado()
        {
            return View("RegistrarEmpleado");
        }

        // Guarda el empleado
        [HttpPost]
        public IActionResult Registrar(Empleado empleado)
        {
            try
            {
                // Validar campos requeridos
                if (string.IsNullOrWhiteSpace(empleado.Nombre) ||
                    string.IsNullOrWhiteSpace(empleado.Apellidos) ||
                    empleado.FechaNacimiento == default ||
                    string.IsNullOrWhiteSpace(empleado.Email) ||
                    string.IsNullOrWhiteSpace(empleado.Dni))
                {
                    ViewBag.Error = "?? Todos los campos obligatorios deben ser completados.";
                    return View("RegistrarEmpleado", empleado);
                }

                // Validar longitud del DNI
                if (empleado.Dni.Length != 8 || !empleado.Dni.All(char.IsDigit))
                {
                    ViewBag.Error = "?? El DNI debe tener exactamente 8 dígitos numéricos.";
                    return View("RegistrarEmpleado", empleado);
                }

                // Validar correo
                if (!empleado.Email.Contains("@"))
                {
                    ViewBag.Error = "?? El correo electrónico no es válido.";
                    return View("RegistrarEmpleado", empleado);
                }

                // Validar duplicados
                bool existe = _context.Empleados.Any(e => e.Dni == empleado.Dni || e.Email == empleado.Email);
                if (existe)
                {
                    ViewBag.Error = "?? El empleado ya se encuentra registrado.";
                    return View("RegistrarEmpleado", empleado);
                }

                // Calcular edad automáticamente
                empleado.Edad = DateTime.Now.Year - empleado.FechaNacimiento.Year;
                if (DateTime.Now.DayOfYear < empleado.FechaNacimiento.DayOfYear)
                    empleado.Edad--;

                // Guardar en la base de datos
                _context.Empleados.Add(empleado);
                _context.SaveChanges();

                ViewBag.Exito = "? Empleado registrado correctamente.";
                ModelState.Clear();
                return View("RegistrarEmpleado");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "? Error al registrar empleado: " + ex.Message;
                return View("RegistrarEmpleado", empleado);
            }
        }
    }
}



