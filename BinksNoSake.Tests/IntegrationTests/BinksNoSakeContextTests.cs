using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BinksNoSake.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace BinksNoSake.Tests.IntegrationTests;
public class BinksNoSakeContextTests
{
    private readonly BinksNoSakeContext _context;

    public BinksNoSakeContextTests()
    {
        var dbOptions = new DbContextOptionsBuilder<BinksNoSakeContext>()
            .UseInMemoryDatabase(databaseName: "BinksNoSake")
            .Options;

        _context = new BinksNoSakeContext(dbOptions);
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldReturnTrue()
    {
        // Arrange
        var pirata = new PirataModel { Nome = "Monkey D. Luffy", Funcao = "Capitão", Objetivo = "Encontrar o One Piece", CapitaoId = 1 };

        // Act
        await _context.Piratas.AddAsync(pirata);
        var result = await _context.SaveChangesAsync();

        // Assert
        Assert.True(result > 0);
    }

    [Fact]
    public async Task Save_SetCreatedValue()
    {
        // Arrange
        var pirata = new PirataModel { Nome = "Monkey D. Luffy", Funcao = "Capitão", DataIngressoTripulacao = DateTime.Now, Objetivo = "Encontrar o One Piece", CapitaoId = 1 };

        // Act
        await _context.Piratas.AddAsync(pirata);
        await _context.SaveChangesAsync();

        // Assert
        Assert.True(pirata.Id != 0);
        Assert.NotNull(pirata.DataIngressoTripulacao);
    }
}
