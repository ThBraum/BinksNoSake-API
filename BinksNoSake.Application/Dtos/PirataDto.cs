using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BinksNoSake.Domain.Models;

namespace BinksNoSake.Application.Dtos;
public class PirataDto
{
    public int PirataId { get; set; }
    [Display(Name = "Nome do Pirata")]
    [Required(ErrorMessage = "O nome do pirata é obrigatório.")]
    [MaxLength(50, ErrorMessage = "O nome deve ter no máximo 50 caracteres")]
    public string Nome { get; set; }
    public string Funcao { get; set; }
    public DateTime DataIngressoTripulacao { get; set; }
    [Display(Name = "Objetivo do Pirata")]
    public string Objetivo { get; set; }

    // Propriedades relacionadas a Capitão
    public int? CapitaoId { get; set; }
    [JsonIgnore] // Para não dar erro de referência circular entre Capitao e Pirata
    public CapitaoDto? Capitao { get; set; }

    // Propriedades relacionadas a Navio
    public int? NavioId { get; set; }
    public NavioDto? Navio { get; set; }
}