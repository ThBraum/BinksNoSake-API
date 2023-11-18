using System.ComponentModel.DataAnnotations;

namespace BinksNoSake.Domain.Models;
public class NavioModel
{
    [Key]
    public int Id { get; set; }
    public string? Nome { get; set; }

    // Relação com Piratas
    public int? PirataId { get; set; }
    public PirataModel? Pirata { get; set; }
}