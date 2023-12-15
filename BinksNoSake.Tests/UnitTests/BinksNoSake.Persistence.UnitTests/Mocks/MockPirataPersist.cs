using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BinksNoSake.Domain.Models;
using BinksNoSake.Persistence.Contratos;
using BinksNoSake.Persistence.Pagination;
using Moq;

namespace BinksNoSake.Tests.UnitTests.BinksNoSake.Application.UnitTests.Mocks;
public class MockPirataPersist
{
    public static Mock<IPirataPersist> GetMockPirataPersist(PageParams? pageParams)
    {
        var piratas = new List<PirataModel>
        {
            new PirataModel
            {
                Id = 1,
                Nome = "Monkey D. Luffy",
                Funcao = "Capitão",
                DataIngressoTripulacao = new DateTime(1997, 5, 5),
                CapitaoId = 1
            },
            new PirataModel
            {
                Id = 2,
                Nome = "Roronoa Zoro",
                Funcao = "Espadachim",
                DataIngressoTripulacao = new DateTime(1997, 11, 11),
                CapitaoId = 1
            },
            new PirataModel
            {
                Id = 3,
                Nome = "Nami",
                Funcao = "Navegadora",
                DataIngressoTripulacao = new DateTime(1998, 7, 3),
                CapitaoId = 1
            },
            new PirataModel
            {
                Id = 4,
                Nome = "Usopp",
                Funcao = "Atirador",
                DataIngressoTripulacao = new DateTime(1998, 4, 1),
                CapitaoId = 1
            },
            new PirataModel
            {
                Id = 5,
                Nome = "Sanji",
                Funcao = "Cozinheiro",
                DataIngressoTripulacao = new DateTime(1998, 3, 2),
                CapitaoId = 1
            },
            new PirataModel
            {
                Id = 6,
                Nome = "Tony Tony Chopper",
                Funcao = "Médico",
                DataIngressoTripulacao = new DateTime(1998, 12, 24),
                CapitaoId = 1
            },
            new PirataModel
            {
                Id = 7,
                Nome = "Nico Robin",
                Funcao = "Arqueóloga",
                DataIngressoTripulacao = new DateTime(1998, 2, 6),
                CapitaoId = 1
            },
            new PirataModel
            {
                Id = 8,
                Nome = "Franky",
                Funcao = "Carpinteiro",
                DataIngressoTripulacao = new DateTime(1998, 3, 9),
                CapitaoId = 1
            },
            new PirataModel
            {
                Id = 9,
                Nome = "Brook",
                Funcao = "Músico",
                DataIngressoTripulacao = new DateTime(1998, 7, 5),
                CapitaoId = 1
            }
        };

        var capitaes = new List<CapitaoModel>
        {
            new CapitaoModel
            {
                Id = 1,
                Nome = "Shanks"
            },
            new CapitaoModel
            {
                Id = 2,
                Nome = "Monkey D. Luffy"
            },
            new CapitaoModel
            {
                Id = 3,
                Nome = "Gol D. Roger"
            },
            new CapitaoModel
            {
                Id = 4,
                Nome = "Barba Branca"
            },
            new CapitaoModel
            {
                Id = 5,
                Nome = "Barba Negra"
            },

        };

        var mockPirataPersist = new Mock<IPirataPersist>();
        var mockCapitaoPersist = new Mock<ICapitaoPersist>();
        var mockGeralPersist = new Mock<IGeralPersist>();


        mockPirataPersist.Setup(p => p.GetAllPiratasAsync(It.IsAny<PageParams>()))
            .ReturnsAsync((PageParams pageParams) =>
            {
                if (piratas == null)
                {
                    return new PageList<PirataModel>(new List<PirataModel>(), 0, pageParams.PageNumber, pageParams.PageSize);
                }

                var filteredPiratas = piratas
                    .Where(p => (p.Nome != null && p.Nome.ToLower().Contains(pageParams.Term.ToLower())) ||
                                (p.Funcao != null && p.Funcao.ToLower().Contains(pageParams.Term.ToLower())) ||
                                (p.Objetivo != null && p.Objetivo.ToLower().Contains(pageParams.Term.ToLower())))
                    .OrderBy(p => p.Id)
                    .ToList();


                return new PageList<PirataModel>(filteredPiratas, filteredPiratas.Count, pageParams.PageNumber, pageParams.PageSize);
            });

        mockPirataPersist.Setup(p => p.GetPirataByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int pirataId) =>
            {
                return piratas.FirstOrDefault(p => p.Id == pirataId);
            });

        mockPirataPersist.Setup(p => p.AddPirataWithExistingCapitaoAsync(It.IsAny<PirataModel>(), It.IsAny<CapitaoModel>()))
            .ReturnsAsync((PirataModel pirata, CapitaoModel capitao) =>
            {
                var capitaoExistente = capitaes.FirstOrDefault(c => c.Nome == capitao.Nome);

                if (capitaoExistente != null)
                {
                    pirata.CapitaoId = capitaoExistente.Id;
                    pirata.Capitao = capitaoExistente;
                }
                else
                {
                    pirata.Capitao = new CapitaoModel
                    {
                        Id = capitao.Id,
                        Nome = capitao.Nome,
                        Piratas = capitao.Piratas,
                        TimoneiroId = capitao.TimoneiroId,
                        Timoneiro = capitao.Timoneiro
                    };
                }

                return pirata;
            });

        return mockPirataPersist;
    }
}