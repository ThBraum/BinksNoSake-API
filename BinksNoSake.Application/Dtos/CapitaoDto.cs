using System.ComponentModel.DataAnnotations;
using BinksNoSake.Domain.Models;

namespace BinksNoSake.Application.Dtos;
public class CapitaoDto
{
    [Key]
    public int CapitaoId { get; set; }
    [Display(Name = "Nome do Capitão")]
    [Required(ErrorMessage = "O nome do capitão é obrigatório.")]
    [MaxLength(50, ErrorMessage = "O nome deve ter no máximo 50 caracteres")]
    public string Nome { get; set; }

    //Relação com Piratas
    public ICollection<PirataModel> Piratas { get; set; }

    //Relação com Timoneiro
    public int? TimoneiroId { get; set; }
    public TimoneiroModel Timoneiro { get; set; }
}