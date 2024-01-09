using System.ComponentModel.DataAnnotations;

namespace BinksNoSake.Domain.Models;
public class PirataModel
{
    [Key]
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? Funcao { get; set; }
    public DateTime? DataIngressoTripulacao { get; set; }
    public string? Objetivo { get; set; }
    public string? ImagemURL { get; set; }

    // Relação com Capitao
    public int? CapitaoId { get; set; }
    public CapitaoModel? Capitao { get; set; }

    // Relação com Navios
    public List<NavioModel>? Navios { get; set; }
}
