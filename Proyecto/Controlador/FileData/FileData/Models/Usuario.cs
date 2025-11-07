namespace FileData.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Usuario1 { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public string Cargo { get; set; } = null!;

    public int? IdEmpleado { get; set; }

    public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>();

    public virtual Empleado? IdEmpleadoNavigation { get; set; }
}
