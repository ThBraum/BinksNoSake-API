using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BinksNoSake.Application.Contratos;
using BinksNoSake.Application.Dtos;
using BinksNoSake.Application.Helpers;
using BinksNoSake.Persistence.Pagination;
using Moq;

namespace BinksNoSake.Tests.IntegrationTests.Mocks;
public class MockPirataService
{
    public static Mock<IPirataService> GetMockPirataService(PageParams? pageParams)
    {
        var piratas = new List<PirataDto>
        {
            new PirataDto
            {
                Id = 1,
                Nome = "Monkey D. Luffy",
                Funcao = "Capitão",
                DataIngressoTripulacao = new DateTime(1997, 5, 5),
                CapitaoId = 1
            },
            new PirataDto
            {
                Id = 2,
                Nome = "Roronoa Zoro",
                Funcao = "Espadachim",
                DataIngressoTripulacao = new DateTime(1997, 11, 11),
                CapitaoId = 1
            },
            new PirataDto
            {
                Id = 3,
                Nome = "Nami",
                Funcao = "Navegadora",
                DataIngressoTripulacao = new DateTime(1998, 7, 3),
                CapitaoId = 1
            },
            new PirataDto
            {
                Id = 4,
                Nome = "Usopp",
                Funcao = "Atirador",
                DataIngressoTripulacao = new DateTime(1998, 4, 1),
                CapitaoId = 1
            },
        };

        var capitaesExistentes = new List<CapitaoDto>
        {
            new CapitaoDto
            {
                Id = 1,
                Nome = "Gol D. Roger"
            },
            new CapitaoDto
            {
                Id = 2,
                Nome = "Monkey D. Luffy"
            }
        };

        var mockPirataService = new Mock<IPirataService>();

        mockPirataService.Setup(p => p.GetAllPiratasAsync(It.IsAny<PageParams>())).ReturnsAsync((PageParams pageParams) =>
        {
            if (pageParams == null || piratas == null)
            {
                return new PageList<PirataDto>(new List<PirataDto>(), 0, 1, 0);
            }

            var filteredPiratas = piratas
                .Where(p => (p.Nome != null && p.Nome.ToLower().Contains(pageParams.Term.ToLower())) ||
                            (p.Funcao != null && p.Funcao.ToLower().Contains(pageParams.Term.ToLower())) ||
                            (p.Objetivo != null && p.Objetivo.ToLower().Contains(pageParams.Term.ToLower())))
                .OrderBy(p => p.Id)
                .ToList();

            return new PageList<PirataDto>(filteredPiratas, filteredPiratas.Count, pageParams.PageNumber, pageParams.PageSize);
        });

        mockPirataService.Setup(p => p.GetPirataByIdAsync(It.IsAny<int>())).
            ReturnsAsync((int pirataId) =>
            {
                return piratas.FirstOrDefault(p => p.Id == pirataId);
            });

        mockPirataService.Setup(p => p.AddPirata(It.IsAny<PirataDto>())).
            ReturnsAsync((PirataDto pirataDto) =>
            {
                var capitaoExistente = capitaesExistentes.FirstOrDefault(c => 
                        (pirataDto.CapitaoId.HasValue && c.Id == pirataDto.CapitaoId.Value) ||
                        (pirataDto.Capitao != null && c.Nome == pirataDto.Capitao.Nome) ||
                        (pirataDto.Capitao != null && pirataDto.Capitao.Id != 0 && pirataDto.Capitao.Id == pirataDto.CapitaoId.Value));


                if (capitaoExistente != null)
                {
                    pirataDto.CapitaoId = capitaoExistente.Id;
                    pirataDto.Capitao = capitaoExistente;
                }
                else if (pirataDto.Capitao != null)
                {
                    pirataDto.Capitao = new CapitaoDto
                    {
                        Id = pirataDto.Capitao.Id,
                        Nome = pirataDto.Capitao.Nome
                    };
                }

                pirataDto.Id = piratas.Max(p => p.Id) + 1;
                piratas.Add(pirataDto);

                return pirataDto;
            });

        mockPirataService.Setup(p => p.DeletePirata(It.IsAny<int>())).
            ReturnsAsync((int pirataId) =>
            {
                var pirata = piratas.FirstOrDefault(p => p.Id == pirataId);
                if (pirata == null) return false;
                piratas.Remove(pirata);
                return true;
            });

        mockPirataService.Setup(p => p.UpdatePirata(It.IsAny<int>(), It.IsAny<PirataDto>())).
            ReturnsAsync((int pirataId, PirataDto pirataDto) =>
            {
                var pirata = piratas.FirstOrDefault(p => p.Id == pirataId);
                if (pirata == null) return null;
                pirata.Nome = pirataDto.Nome;
                pirata.Funcao = pirataDto.Funcao;
                pirata.Objetivo = pirataDto.Objetivo;
                pirata.DataIngressoTripulacao = pirataDto.DataIngressoTripulacao;
                pirata.CapitaoId = pirataDto.CapitaoId;
                pirata.Capitao = pirataDto.Capitao;
                return pirata;
            });


        return mockPirataService;
    }
}