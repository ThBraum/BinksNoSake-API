using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BinksNoSake.Application.Contratos;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace BinksNoSake.Tests.IntegrationTests;
public class TestClient
{
    public static HttpClient GetTestClient(IPirataService mockPirataService)
    {
        return new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault( // substitui o serviço real por um mock
                    d => d.ServiceType == typeof(IPirataService));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddSingleton(mockPirataService);
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
            });
        }).CreateClient();
    }
}