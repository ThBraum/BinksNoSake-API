using System.ComponentModel.DataAnnotations;

namespace BinksNoSake.Domain.Models;
public class PirataModel
{
    [Key]
    public int PirataId { get; set; }
    public string Nome { get; set; }
    public string Funcao { get; set; }
    public DateTime DataIngressoTripulacao { get; set; }
    public string Objetivo { get; set; }

    // Relação com Capitao
    public CapitaoModel Capitao { get; set; }

    // Relação com Navios
    public ICollection<NavioModel> Navios { get; set; }
}
