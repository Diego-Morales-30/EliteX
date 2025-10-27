using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using FileData.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace FileData.Controllers
{
    public class AccountController : Controller
    {
        private readonly BdDataFileContext _context;
        private readonly IConfiguration _config;

        public AccountController(BdDataFileContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // --- LOGIN ---
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string usuario, string password, string accion)
        {
            ViewBag.UsuarioInput = usuario;
            ViewBag.PasswordInput = password;

            var user = _context.Usuarios
                .Include(u => u.IdEmpleadoNavigation)
                .FirstOrDefault(u => u.Usuario1 == usuario && u.Contrasena == password);

            if (user == null)
            {
                ViewBag.Error = "Usuario o contraseña incorrectos";
                return View();
            }

            if (accion == "validar")
            {
                ViewBag.NombreCompleto = $"{user.IdEmpleadoNavigation.Nombre} {user.IdEmpleadoNavigation.Apellidos}";
                ViewBag.Cargo = user.Cargo;
                return View();
            }

            if (accion == "acceder")
                return RedirectToAction("Index", "Home");

            return View();
        }

        // --- ENVIAR CÓDIGO DE VERIFICACIÓN ---
        [HttpPost]
        public IActionResult EnviarCodigo(string correo)
        {
            var usuario = _context.Usuarios
                .Include(u => u.IdEmpleadoNavigation)
                .FirstOrDefault(u => u.IdEmpleadoNavigation.Email == correo);

            if (usuario == null)
                return Json(new { success = false, message = "El correo ingresado no está registrado." });

            var codigo = new Random().Next(100000, 999999).ToString();
            usuario.Contrasena = $"temp:{codigo}";
            _context.SaveChanges();

            var remitente = _config["EmailSettings:Remitente"];
            var clave = _config["EmailSettings:Clave"];
            var smtpGmail = _config["EmailSettings:SmtpGmail"];
            var smtpOutlook = _config["EmailSettings:SmtpOutlook"];
            var puerto = int.Parse(_config["EmailSettings:Puerto"]);

            try
            {
                string smtpHost = remitente.EndsWith("@gmail.com") ? smtpGmail :
                                 remitente.EndsWith("@outlook.com") || remitente.EndsWith("@hotmail.com") ? smtpOutlook :
                                 throw new Exception("Dominio de correo no soportado.");

                SmtpClient smtp = new SmtpClient(smtpHost)
                {
                    Port = puerto,
                    Credentials = new NetworkCredential(remitente, clave),
                    EnableSsl = true
                };

                string asunto = "Recuperación de contraseña - Código de verificación";
                string cuerpo = $@"
                    <h2>Recuperación de Contraseña</h2>
                    <p>Estimado/a <strong>{usuario.IdEmpleadoNavigation.Nombre}</strong>,</p>
                    <p>Tu código de verificación es:</p>
                    <h3 style='color:#d4af37;'>{codigo}</h3>
                    <p>Por favor, introdúcelo en el sistema para continuar.</p>
                    <br/>
                    <p>Atentamente,<br/>Soporte del Sistema</p>
                ";

                MailMessage mensaje = new MailMessage(remitente, correo, asunto, cuerpo)
                {
                    IsBodyHtml = true
                };

                smtp.Send(mensaje);

                return Json(new { success = true, message = "Código enviado correctamente a tu correo registrado." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al enviar el correo: " + ex.Message });
            }
        }

        // --- VERIFICAR CÓDIGO ---
        [HttpPost]
        public IActionResult VerificarCodigo(string correo, string codigo)
        {
            var usuario = _context.Usuarios
                .Include(u => u.IdEmpleadoNavigation)
                .FirstOrDefault(u => u.IdEmpleadoNavigation.Email == correo);

            if (usuario == null)
                return Json(new { success = false, message = "Correo no válido." });

            if (usuario.Contrasena != $"temp:{codigo}")
                return Json(new { success = false, message = "Código incorrecto o expirado." });

            return Json(new { success = true, message = "Código verificado correctamente." });
        }

        // --- CAMBIAR CONTRASEÑA ---
        [HttpPost]
        public IActionResult CambiarContrasena(string correo, string nuevaContrasena)
        {
            var usuario = _context.Usuarios
                .Include(u => u.IdEmpleadoNavigation)
                .FirstOrDefault(u => u.IdEmpleadoNavigation.Email == correo);

            if (usuario == null)
                return Json(new { success = false, message = "Correo no encontrado." });

            usuario.Contrasena = nuevaContrasena;
            _context.SaveChanges();

            return Json(new { success = true, message = "Contraseña actualizada correctamente." });
        }

        // --- OBTENER EMPLEADOS SIN USUARIO ---
        [HttpGet]
        public IActionResult ObtenerEmpleados()
        {
            var empleados = _context.Empleados
                .Include(e => e.Usuario) // relación 1:1
                .Where(e => e.Usuario == null) // solo empleados que no tienen usuario aún
                .Select(e => new
                {
                    idEmpleado = e.IdEmpleado,
                    apellidos = e.Apellidos,
                    dni = e.Dni
                })
                .ToList();

            return Json(empleados);
        }

        // --- CREAR NUEVO USUARIO ---
        [HttpPost]
        public IActionResult CrearUsuario(int idEmpleado, string contrasena, string cargo, string codigoAdmin)
        {
            try
            {
                if (codigoAdmin != "3003200")
                    return Json(new { success = false, message = "Código de administrador incorrecto." });

                var regex = new Regex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^\w\d\s]).{8,}$");
                if (!regex.IsMatch(contrasena))
                    return Json(new
                    {
                        success = false,
                        message = "La contraseña debe tener al menos 8 caracteres, una mayúscula, un número y un símbolo especial."
                    });

                var empleado = _context.Empleados.FirstOrDefault(e => e.IdEmpleado == idEmpleado);
                if (empleado == null)
                    return Json(new { success = false, message = "Empleado no encontrado." });

                string nuevoUsuario = "DF" + empleado.Dni;

                var existe = _context.Usuarios.Any(u => u.Usuario1 == nuevoUsuario);
                if (existe)
                    return Json(new { success = false, message = "Ya existe un usuario para este empleado." });

                var nuevo = new Usuario
                {
                    Usuario1 = nuevoUsuario,
                    Contrasena = contrasena,
                    Cargo = cargo,
                    IdEmpleado = idEmpleado
                };

                _context.Usuarios.Add(nuevo);
                _context.SaveChanges();

                return Json(new { success = true, message = "Usuario creado correctamente: " + nuevoUsuario });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al crear usuario: " + ex.InnerException?.Message });
            }
        }
    }
}
