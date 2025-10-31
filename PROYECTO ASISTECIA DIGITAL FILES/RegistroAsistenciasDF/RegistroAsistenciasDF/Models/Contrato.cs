using System;
using System.Collections.Generic;

namespace RegistroAsistenciasDF.Models;

public partial class Contrato
{
    public int IdContrato { get; set; }

    public int IdEmpleado { get; set; }

    public int IdTipoHorario { get; set; }

    public decimal Sueldo { get; set; }

    public virtual Empleado IdEmpleadoNavigation { get; set; } = null!;

    public virtual TipoHorario IdTipoHorarioNavigation { get; set; } = null!;
}
