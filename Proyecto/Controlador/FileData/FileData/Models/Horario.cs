using System;
using System.Collections.Generic;

namespace FileData.Models;

public partial class Horario
{
    public int IdHorario { get; set; }

    public int IdEmpleado { get; set; }

    public DateOnly FechaInicio { get; set; }

    public TimeOnly HoraEntrada { get; set; }

    public TimeOnly? HoraSalida { get; set; }

    public string? Observaciones { get; set; }

    public int UsuarioRegistro { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual Empleado IdEmpleadoNavigation { get; set; } = null!;

    public virtual Usuario UsuarioRegistroNavigation { get; set; } = null!;
}
