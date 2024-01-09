using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BinksNoSake.Domain.Models;

namespace BinksNoSake.Application.Dtos;
public class CapitaoDto
{
    [Key]
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? ImagemURL { get; set; }
    // Relação com Piratas
    public List<PirataDto>? Piratas { get; set; }

    // Relação com Timoneiro
    public int? TimoneiroId { get; set; }
    public TimoneiroDto? Timoneiro { get; set; }
}