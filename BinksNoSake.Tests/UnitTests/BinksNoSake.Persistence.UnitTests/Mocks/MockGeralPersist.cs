using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BinksNoSake.Persistence.Contratos;
using Moq;

namespace BinksNoSake.Tests.UnitTests.BinksNoSake.Application.UnitTests.Mocks;
public class MockGeralPersist
{
    public static Mock<IGeralPersist> GetMockGeralPersist()
    {
        var mockGeralPersist = new Mock<IGeralPersist>();

        mockGeralPersist.Setup(g => g.Add(It.IsAny<object>())).Callback<object>(entity => mockGeralPersist.Object.Add(entity));
        mockGeralPersist.Setup(g => g.Update(It.IsAny<object>())).Callback<object>(entity => mockGeralPersist.Object.Update(entity));
        mockGeralPersist.Setup(g => g.Delete(It.IsAny<object>())).Callback<object>(entity => mockGeralPersist.Object.Delete(entity));
        mockGeralPersist.Setup(g => g.DeleteRange(It.IsAny<object[]>())).Callback<object[]>(entityArray => mockGeralPersist.Object.DeleteRange(entityArray));
        mockGeralPersist.Setup(g => g.Detach(It.IsAny<object>())).Callback<object>(entity => mockGeralPersist.Object.Detach(entity));
        mockGeralPersist.Setup(g => g.SaveChangesAsync()).ReturnsAsync(() => true);

        return mockGeralPersist;
    }
}