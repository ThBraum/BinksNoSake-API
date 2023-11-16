using System.ComponentModel.DataAnnotations;
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
    [DataType(DataType.DateTime)]
    public DateTime DataIngressoTripulacao { get; set; }
    [Display(Name = "Objetivo do Pirata")]
    public string Objetivo { get; set; }

    //Relação com Capitao
    public CapitaoModel Capitao { get; set; }

    //Relação com Navios
    public ICollection<NavioModel> Navios { get; set; }
}