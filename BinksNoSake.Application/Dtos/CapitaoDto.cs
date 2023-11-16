using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BinksNoSake.Domain.Models;

namespace BinksNoSake.Application.Dtos;
public class CapitaoDto
{
    [Key]
    public int CapitaoId { get; set; }
    [Display(Name = "Nome do Capitão")]
    [MaxLength(50, ErrorMessage = "O nome deve ter no máximo 50 caracteres")]
    public string Nome { get; set; }

    //Relação com Timoneiro
    /* public int? TimoneiroId { get; set; }
    public TimoneiroDto? Timoneiro { get; set; } */
    //Relação com Piratas
    public List<PirataDto>? Piratas { get; set; }

    //Relação com Timoneiro
    public int? TimoneiroId { get; set; }
    [JsonIgnore]
    public TimoneiroDto Timoneiro { get; set; }
}