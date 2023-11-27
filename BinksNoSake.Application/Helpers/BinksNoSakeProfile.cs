using AutoMapper;
using BinksNoSake.Application.Dtos;
using BinksNoSake.Domain.Identity;
using BinksNoSake.Domain.Models;

namespace BinksNoSake.Application.Helpers;
public class BinksNoSakeProfile : Profile
{
    public BinksNoSakeProfile()
    {
        CreateMap<Account, AccountDto>().ReverseMap();
        CreateMap<Account, AccountLoginDto>().ReverseMap();
        CreateMap<Account, AccountUpdateDto>().ReverseMap();
        
        CreateMap<PirataModel, PirataDto>()
            .ForMember(dest => dest.CapitaoId, opt => opt.MapFrom(src => src.CapitaoId))
            .ForMember(dest => dest.Capitao, opt => opt.MapFrom(src => src.Capitao))
            .ReverseMap();

        CreateMap<CapitaoModel, CapitaoDto>().ReverseMap();
        CreateMap<CapitaoModel, CapitaoDto>().ReverseMap();
        CreateMap<NavioModel, NavioDto>().ReverseMap();
        CreateMap<TimoneiroModel, TimoneiroDto>().ReverseMap();
    }
}