using System;
using System.Collections.Generic;

namespace RegistroAsistenciasDF.Models;

public partial class TipoHorario
{
    public int IdTipoHorario { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Contrato> Contratos { get; set; } = new List<Contrato>();
}
