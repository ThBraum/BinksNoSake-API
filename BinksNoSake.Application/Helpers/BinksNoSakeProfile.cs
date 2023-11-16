using AutoMapper;
using BinksNoSake.Application.Dtos;
using BinksNoSake.Domain.Models;

namespace BinksNoSake.Application.Helpers;
public class BinksNoSakeProfile : Profile
{
    public BinksNoSakeProfile()
    {
        CreateMap<PirataModel, PirataDto>().ReverseMap()
                .ForMember(dest => dest.Capitao, opt => opt.Ignore())
                .ForMember(dest => dest.Navios, opt => opt.Ignore()); //Ignore para não dar erro de referência circular
        CreateMap<CapitaoModel, CapitaoDto>().ReverseMap();
        CreateMap<NavioModel, NavioDto>().ReverseMap();
        CreateMap<TimoneiroModel, TimoneiroDto>().ReverseMap();
    }
}