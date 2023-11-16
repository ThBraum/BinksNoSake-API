using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
    public CapitaoModel Capitao { get; set; }
}