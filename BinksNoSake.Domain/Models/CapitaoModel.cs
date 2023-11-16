using System.ComponentModel.DataAnnotations;

namespace BinksNoSake.Domain.Models;
public class CapitaoModel
{
    [Key]
    public int CapitaoId { get; set; }
    public string Nome { get; set; }

    // Relação com Piratas
    public ICollection<PirataModel> Piratas { get; set; }

    // Relação com Timoneiro
    public int? TimoneiroId { get; set; }
    public TimoneiroModel Timoneiro { get; set; }
}
