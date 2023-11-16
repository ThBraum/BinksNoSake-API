using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BinksNoSake.Domain.Models;
public class TimoneiroModel
{
    [Key]
    public int TimoneiroId { get; set; }
    public string Nome { get; set; }

    // Relação com Capitao
    public int? CapitaoId { get; set; }
    public CapitaoModel Capitao { get; set; }
}