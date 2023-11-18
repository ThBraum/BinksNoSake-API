using System.ComponentModel.DataAnnotations;
using BinksNoSake.Domain.Models;

namespace BinksNoSake.Application.Dtos;
public class NavioDto
{
    [Key]
    public int Id { get; set; }
    public string? Nome { get; set; }

    // Relação com Piratas
    public int? PirataId { get; set; }
    public PirataDto? Pirata { get; set; }
}