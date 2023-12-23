using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BinksNoSake.Application.Helpers;
using BinksNoSake.Domain.Models;
using BinksNoSake.Persistence.Contratos;
using BinksNoSake.Persistence.Pagination;
using BinksNoSake.Tests.UnitTests.BinksNoSake.Application.UnitTests.Mocks;
using Moq;
using Shouldly;
using Xunit;

namespace BinksNoSake.Tests.UnitTests.BinksNoSake.Persistence.UnitTests;
public class CapitaoPersistTests
{
    private Mock<IPirataPersist> _mockPirataPersist;
    private Mock<ICapitaoPersist> _mockCapitaoPersist;
    private Mock<IGeralPersist> _mockGeralPersist;
    private IMapper _mapper;

    public CapitaoPersistTests()
    {
        var pageParams = new PageParams
        {
            PageNumber = 1,
            PageSize = 10
        };

        _mockPirataPersist = MockPirataPersist.GetMockPirataPersist(pageParams);
        _mockCapitaoPersist = MockCapitaoPersist.GetMockCapitaoPersist(pageParams);
        _mockGeralPersist = MockGeralPersist.GetMockGeralPersist();

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<BinksNoSakeProfile>();
        });
    }

    [Fact]
    public async Task TestGetAllCapitaes_ReturnsCapitaes()
    {
        var pageParams = new PageParams
        {
            PageNumber = 1,
            PageSize = 10
        };
        _mockCapitaoPersist = MockCapitaoPersist.GetMockCapitaoPersist(pageParams);

        var capitaoPersist = _mockCapitaoPersist.Object;
        var result = await capitaoPersist.GetAllCapitaesAsync(pageParams);

        Assert.NotNull(result);
        result.ShouldBeOfType<PageList<CapitaoModel>>();
        
        Assert.Equal("Monkey D. Luffy", result[0].Nome);
        Assert.Equal("Gol D. Roger", result[1].Nome);
        Assert.Equal("Shanks", result[2].Nome);

        Assert.Equal(1, result.CurrentPage);
        Assert.Equal(10, result.PageSize);
        Assert.Equal(1, result.TotalPages);

        result.TotalCount.ShouldBe(5);
    }

    [Fact]
    public async Task TestGetCapitaoById_ReturnsCapitao()
    {
        var pageParams = new PageParams
        {
            PageNumber = 1,
            PageSize = 10
        };
        var capitaoId = 2;
        
        _mockCapitaoPersist = MockCapitaoPersist.GetMockCapitaoPersist(pageParams);
        var result = await _mockCapitaoPersist.Object.GetCapitaoByIdAsync(capitaoId);

        Assert.NotNull(result);
        result.ShouldBeOfType<CapitaoModel>();
        
        Assert.Equal(2, result.Id);
        Assert.Equal("Gol D. Roger", result.Nome);
    }
}