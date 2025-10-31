using System;
using System.Collections.Generic;

namespace RegistroAsistenciasDF.Models;

public partial class Empleado
{
    public int IdEmpleado { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public int? IdUsuario { get; set; }

    public virtual ICollection<Contrato> Contratos { get; set; } = new List<Contrato>();

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
