using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BinksNoSake.Domain.Models;
using BinksNoSake.Persistence.Contratos;
using Moq;

namespace BinksNoSake.Tests.UnitTests.BinksNoSake.Application.UnitTests.Mocks;
public class MockCapitaoPersist
{
    public static Mock<ICapitaoPersist> GetMockCapitaoPersist()
    {
        var capitaes = new List<CapitaoModel>
        {
            new CapitaoModel
            {
                Id = 1,
                Nome = "Monkey D. Luffy"
            },
            new CapitaoModel
            {
                Id = 2,
                Nome = "Gol D. Roger"
            },
            new CapitaoModel
            {
                Id = 3,
                Nome = "Shanks"
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

        var mockCapitaoPersist = new Mock<ICapitaoPersist>();
        
        mockCapitaoPersist.Setup(c => c.GetAllCapitaesAsync()).Returns(Task.FromResult(capitaes.ToArray()));
        
        mockCapitaoPersist.Setup(c => c.GetCapitaoByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int capitaoId) =>
            {
                return capitaes.FirstOrDefault(c => c.Id == capitaoId);
            });

        mockCapitaoPersist.Setup(c => c.GetCapitaoByNomeAsync(It.IsAny<string>()))
            .ReturnsAsync((string nome) =>
            {
                return capitaes.FirstOrDefault(c => c.Nome == nome);
            });

        return mockCapitaoPersist;
    }
}