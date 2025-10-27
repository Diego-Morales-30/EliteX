using System;
using System.Collections.Generic;

namespace FileData.Models;

public partial class Empleado
{
    public int IdEmpleado { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    public int? Edad { get; set; }

    public string? EstadoCivil { get; set; }

    public string? Email { get; set; }

    public string? Telefono { get; set; }

    public string Dni { get; set; } = null!;

    public virtual Usuario? Usuario { get; set; }
}
