using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BinksNoSake.Domain.Models;

namespace BinksNoSake.Application.Dtos;
public class PirataDto
{
    [Key]
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? Funcao { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? DataIngressoTripulacao { get; set; }
    public string? Objetivo { get; set; }
    public string? ImagemURL { get; set; }

    // Relação com Capitao
    public int? CapitaoId { get; set; }
    public CapitaoDto? Capitao { get; set; }

    // Relação com Navios
    public IEnumerable<NavioDto>? Navios { get; set; }
}