using System.ComponentModel.DataAnnotations;
using BinksNoSake.Domain.Models;

namespace BinksNoSake.Application.Dtos;
public class NavioDto
{
    [Key]
    public int NavioId { get; set; }
    [Display(Name = "Nome do Navio")]
    public string Nome { get; set; }

    //Relação com Piratas
    public PirataModel Pirata { get; set; }
}