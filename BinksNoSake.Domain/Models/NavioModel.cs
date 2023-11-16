using System.ComponentModel.DataAnnotations;

namespace BinksNoSake.Domain.Models;
public class NavioModel
{
    [Key]
    public int NavioId { get; set; }
    public string Nome { get; set; }

    // Relação com Piratas
    public PirataModel Pirata { get; set; }
}