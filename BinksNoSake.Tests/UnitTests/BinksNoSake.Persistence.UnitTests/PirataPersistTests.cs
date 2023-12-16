using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BinksNoSake.Application.Helpers;
using BinksNoSake.Application.Services;
using BinksNoSake.Domain.Models;
using BinksNoSake.Persistence.Contratos;
using BinksNoSake.Persistence.Pagination;
using BinksNoSake.Persistence.Persistence;
using BinksNoSake.Tests.UnitTests.BinksNoSake.Application.UnitTests.Mocks;
using Moq;
using Moq.Language.Flow;
using Shouldly;
using Xunit;

namespace BinksNoSake.Tests.UnitTests.BinksNoSake.Persistence.UnitTests;
public class PirataPersist
{
    private Mock<IPirataPersist> _mockPirataPersist;
    private Mock<ICapitaoPersist> _mockCapitaoPersist;
    private Mock<IGeralPersist> _mockGeralPersist;
    private IMapper _mapper;

    public PirataPersist()
    {
        var pageParams = new PageParams
        {
            PageNumber = 1,
            PageSize = 10
        };

        _mockPirataPersist = MockPirataPersist.GetMockPirataPersist(pageParams);
        _mockCapitaoPersist = MockCapitaoPersist.GetMockCapitaoPersist();
        _mockGeralPersist = MockGeralPersist.GetMockGeralPersist();

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<BinksNoSakeProfile>();
        });
        _mapper = mockMapper.CreateMapper();
    }

    [Fact]
    public async Task TestGetAllPiratas_ReturnsPiratas()
    {
        var pageParams = new PageParams
        {
            PageNumber = 1,
            PageSize = 10
        };

        _mockPirataPersist = MockPirataPersist.GetMockPirataPersist(pageParams);
        var pirataPersist = _mockPirataPersist.Object;
        var result = await pirataPersist.GetAllPiratasAsync(pageParams);

        Assert.NotNull(result);
        result.ShouldBeOfType<PageList<PirataModel>>();
        
        Assert.Equal(1, result.CurrentPage);
        Assert.Equal(10, result.PageSize);
        Assert.Equal(1, result.TotalPages);
        
        result.TotalCount.ShouldBe(9);
    }

    [Fact]
    public async Task TestGetPirataById_ReturnsPirata()
    {
        var pageParams = new PageParams
        {
            PageNumber = 1,
            PageSize = 5
        };
        
        var pirataId = 1;

        _mockPirataPersist = MockPirataPersist.GetMockPirataPersist(pageParams);
        var result = await _mockPirataPersist.Object.GetPirataByIdAsync(pirataId);

        Assert.NotNull(result);
        result.ShouldBeOfType<PirataModel>();

        Assert.Equal("Monkey D. Luffy", result.Nome);
        Assert.Equal("Capitão", result.Funcao);
    }

    [Fact]
    public async Task TestGetPirataByTerm_ReturnsPirata()
    {
        var pageParams = new PageParams
        {
            Term = "Luffy",
            PageNumber = 1,
            PageSize = 10
        };

        _mockPirataPersist = MockPirataPersist.GetMockPirataPersist(pageParams);
        var pirataPersist = _mockPirataPersist.Object;
        var result = await pirataPersist.GetAllPiratasAsync(pageParams);

        Assert.NotNull(result);
        result.ShouldBeOfType<PageList<PirataModel>>();

        Assert.Equal(1, result.CurrentPage);
        Assert.Equal(10, result.PageSize);
        Assert.Equal(1, result.TotalPages);
        Assert.Equal(1, result.TotalCount);

        Assert.Equal("Monkey D. Luffy", result.FirstOrDefault().Nome);
        Assert.Equal("Capitão", result.FirstOrDefault().Funcao);
    }

    [Fact]
    public async Task AddPirataWithNewCapitao_ReturnsPirata()
    {
        var pirata = new PirataModel
        {
            Nome = "Roronoa Zoro",
            Funcao = "Espadachim",
            Objetivo = "Ser o melhor espadachim do mundo",
            Capitao = new CapitaoModel
            {
                Nome = "Portgas D. Ace"
            }
        };

        _mockPirataPersist = MockPirataPersist.GetMockPirataPersist(pageParams: null);
        var pirataPersist = _mockPirataPersist.Object;
        var result = await pirataPersist.AddPirataWithExistingCapitaoAsync(pirata, pirata.Capitao);

        Assert.NotNull(result);
        result.ShouldBeOfType<PirataModel>();
        
        Assert.Equal("Roronoa Zoro", result.Nome);
        Assert.Equal("Espadachim", result.Funcao);
        Assert.Equal("Ser o melhor espadachim do mundo", result.Objetivo);
        
        Assert.Equal("Portgas D. Ace", result.Capitao.Nome);
    }

    [Fact]
    public async Task TestAddPirataWithExistingCapitao_ReturnsPirata()
    {
        var pirata = new PirataModel
        {
            Nome = "Roronoa Zoro",
            Funcao = "Espadachim",
            Objetivo = "Ser o melhor espadachim do mundo",
            Capitao = new CapitaoModel
            {
                Nome = "Monkey D. Luffy"
            }
        };

        

        _mockPirataPersist = MockPirataPersist.GetMockPirataPersist(pageParams: null);
        var pirataPersist = _mockPirataPersist.Object;
        var result = await pirataPersist.AddPirataWithExistingCapitaoAsync(pirata, pirata.Capitao);

        Assert.NotNull(result);
        result.ShouldBeOfType<PirataModel>();

        Assert.Equal("Roronoa Zoro", result.Nome);
        Assert.Equal("Espadachim", result.Funcao);
        Assert.Equal("Ser o melhor espadachim do mundo", result.Objetivo);

        Assert.Equal("Monkey D. Luffy", result.Capitao.Nome);
        Assert.Equal(2, result.CapitaoId);
    }

}