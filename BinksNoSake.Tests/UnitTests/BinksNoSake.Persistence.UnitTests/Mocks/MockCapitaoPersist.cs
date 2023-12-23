using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BinksNoSake.Domain.Models;
using BinksNoSake.Persistence.Contratos;
using BinksNoSake.Persistence.Pagination;
using Moq;

namespace BinksNoSake.Tests.UnitTests.BinksNoSake.Application.UnitTests.Mocks;
public class MockCapitaoPersist
{
    public static Mock<ICapitaoPersist> GetMockCapitaoPersist(PageParams? pageParams)
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
        
        mockCapitaoPersist.Setup(c => c.GetAllCapitaesAsync(It.IsAny<PageParams>()))
            .ReturnsAsync((PageParams pageParams) => 
            {
                if (capitaes == null)
                {
                    return new PageList<CapitaoModel>(new List<CapitaoModel>(), 0, pageParams.PageNumber, pageParams.PageSize);
                }

                var filteredCapitaes = capitaes
                    .Where(c => c.Nome.ToLower().Contains(pageParams.Term.ToLower()))
                    .OrderBy(c => c.Id)
                    .ToList();

                return new PageList<CapitaoModel>(filteredCapitaes, filteredCapitaes.Count(), pageParams.PageNumber, pageParams.PageSize);
            });
        
        mockCapitaoPersist.Setup(c => c.GetCapitaoByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int capitaoId) =>
            {
                return capitaes.FirstOrDefault(c => c.Id == capitaoId);
            });

        return mockCapitaoPersist;
    }
}