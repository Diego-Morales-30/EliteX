using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FileData.Models;

public partial class Empleado
{
    public int IdEmpleado { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(50, ErrorMessage = "El nombre no debe superar los 50 caracteres.")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "Los apellidos son obligatorios.")]
    [StringLength(100, ErrorMessage = "Los apellidos no deben superar los 100 caracteres.")]
    public string Apellidos { get; set; } = null!;

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    public DateOnly FechaNacimiento { get; set; }

    [Range(18, 99, ErrorMessage = "La edad debe estar entre 18 y 99 años.")]
    public int? Edad { get; set; }

    [Required(ErrorMessage = "Seleccione un estado civil.")]
    public string? EstadoCivil { get; set; }

    [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido.")]
    public string? Email { get; set; }

    [RegularExpression(@"^\d{9}$", ErrorMessage = "El teléfono debe tener 9 dígitos.")]
    public string? Telefono { get; set; }

    [Required(ErrorMessage = "El DNI es obligatorio.")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe tener 8 dígitos.")]
    public string Dni { get; set; } = null!;

    public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>();

    public virtual Usuario? Usuario { get; set; }
}
