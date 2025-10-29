using Microsoft.AspNetCore.Mvc;

namespace TuProyecto.Controllers
{
    public class ConfiguracionController : Controller
    {
        [HttpGet]
        public IActionResult RegistrarEmpleado()
        {
            // Retorna la vista que está en Views/Configuracion/RegistrarEmpleado.cshtml
            return View();
        }
    }
}


