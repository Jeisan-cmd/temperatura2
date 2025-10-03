using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace websimulador.Models;

[Table("temperaturas")] // 👈 nombre exacto de la tabla en Supabase
public class Temperatura : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Column("valor")]
    public double Valor { get; set; }

    [Column("fecha")]
    public DateTime Fecha { get; set; }
}
