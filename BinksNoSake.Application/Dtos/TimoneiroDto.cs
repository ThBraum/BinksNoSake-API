using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BinksNoSake.Domain.Models;

namespace BinksNoSake.Application.Dtos;
public class TimoneiroDto
{
    [Key]
    public int TimoneiroId { get; set; }
    [Display(Name = "Nome do Timoneiro")]
    public string Nome { get; set; }

    //Relação com Capitao
    public int? CapitaoId { get; set; }
    [JsonIgnore]
    public CapitaoDto Capitao { get; set; }
}