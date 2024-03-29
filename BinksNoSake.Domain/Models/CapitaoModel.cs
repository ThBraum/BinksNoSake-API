using System.ComponentModel.DataAnnotations;

namespace BinksNoSake.Domain.Models;
public class CapitaoModel
{
    [Key]
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? ImagemURL { get; set; }

    // Relação com Piratas
    public List<PirataModel>? Piratas { get; set; }

    // Relação com Timoneiro
    public int? TimoneiroId { get; set; }
    public TimoneiroModel? Timoneiro { get; set; }
}
