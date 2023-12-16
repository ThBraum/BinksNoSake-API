using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Diagnostics;
using System.Net.Http.Json;
using BinksNoSake.Application.Dtos;
using Microsoft.OpenApi.Any;
using AutoMapper;
using BinksNoSake.Domain.Models;
using Xunit;
using BinksNoSake.Tests.IntegrationTests.Mocks;
using Microsoft.Extensions.Hosting;
using BinksNoSake.Persistence.Pagination;
using Moq;
using BinksNoSake.Application.Contratos;
using BinksNoSake.Application.Helpers;
using Shouldly;
using Xunit;

namespace BinksNoSake.Tests.IntegrationTests.Controllers;
public class PirataControllerIntegrationTests
{
    [Fact]
    public async Task TestGetPiratas_ReturnsPiratas()
    {
        var pageParams = new PageParams
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Arrange
        var mockPirataService = MockPirataService.GetMockPirataService(pageParams);
        var client = TestClient.GetTestClient(mockPirataService.Object);

        // Act
        var response = await client.GetAsync("/Pirata");
        var result = await response.Content.ReadFromJsonAsync<PirataDto[]>();

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        response.ShouldBeOfType<HttpResponseMessage>();
        result.ShouldBeOfType<PirataDto[]>();

        Assert.NotNull(result);
        Assert.Equal(4, result.Length);
        Assert.Equal("Monkey D. Luffy", result[0].Nome);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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

        // Arrange
        var mockPirataService = MockPirataService.GetMockPirataService(pageParams);
        var client = TestClient.GetTestClient(mockPirataService.Object);

        // Act
        var response = await client.GetAsync("/Pirata?term=Luffy");
        var result = await response.Content.ReadFromJsonAsync<PirataDto[]>();

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        response.ShouldBeOfType<HttpResponseMessage>();
        result.ShouldBeOfType<PirataDto[]>();

        Assert.NotNull(result);
        Assert.Equal(1, result.Length);
        Assert.Equal("Monkey D. Luffy", result[0].Nome);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }


    [Fact]
    public async Task TestGetPirataById_ReturnsPirata()
    {
        var pageParams = new PageParams
        {
            PageNumber = 1,
            PageSize = 5
        };
        // Arrange
        var mockPirataService = MockPirataService.GetMockPirataService(pageParams);
        var client = TestClient.GetTestClient(mockPirataService.Object);

        // Act
        var response = await client.GetAsync("/Pirata/2");
        var result = await response.Content.ReadFromJsonAsync<PirataDto>();

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        response.ShouldBeOfType<HttpResponseMessage>();
        result.ShouldBeOfType<PirataDto>();

        Assert.NotNull(result);
        Assert.Equal("Roronoa Zoro", result.Nome);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task TestGetNonExistingPirataById_ReturnsNoContent()
    {
        var pageParams = new PageParams
        {
            PageNumber = 1,
            PageSize = 5
        };
        // Arrange
        var mockPirataService = MockPirataService.GetMockPirataService(pageParams);
        var client = TestClient.GetTestClient(mockPirataService.Object);

        // Act
        var response = await client.GetAsync("/Pirata/0");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task TestGetNonExistingPirataByTerm_ReturnsEmptyArray()
    {
        var pageParams = new PageParams
        {
            Term = "Teste",
            PageNumber = 1,
            PageSize = 5
        };
        // Arrange
        var mockPirataService = MockPirataService.GetMockPirataService(pageParams);
        var client = TestClient.GetTestClient(mockPirataService.Object);

        // Act
        var response = await client.GetAsync("/Pirata?term=Teste");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("[]", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task TestPostPirataWithoutCapitao_ReturnsPirata()
    {
        // Arrange
        var mockPirataService = MockPirataService.GetMockPirataService(null);
        var client = TestClient.GetTestClient(mockPirataService.Object);

        // Act
        var response = await client.PostAsJsonAsync("/Pirata", new PirataDto
        {
            Nome = "TesteSemCapitao",
            Funcao = "TesteSemCapitao",
            DataIngressoTripulacao = DateTime.Now,
            Objetivo = "TesteSemCapitao",
            Capitao = null
        });
        var result = await response.Content.ReadFromJsonAsync<PirataDto>();

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        response.ShouldBeOfType<HttpResponseMessage>();
        result.ShouldBeOfType<PirataDto>();

        Assert.NotNull(result);
        Assert.Equal("TesteSemCapitao", result.Nome);
        Assert.Equal(null, result.Capitao);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task TestPostPirataWithNewCapitao_ReturnsPirata()
    {
        // Arrange
        var mockPirataService = MockPirataService.GetMockPirataService(null);
        var client = TestClient.GetTestClient(mockPirataService.Object);

        // Act
        var response = await client.PostAsJsonAsync("/Pirata", new PirataDto
        {
            Nome = "TesteComNovoCapitao",
            Funcao = "TesteComNovoCapitao",
            DataIngressoTripulacao = DateTime.Now,
            Objetivo = "TesteComNovoCapitao",
            Capitao = new CapitaoDto
            {
                Nome = "TesteComNovoCapitao"
            }
        });
        var result = await response.Content.ReadFromJsonAsync<PirataDto>();

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        response.ShouldBeOfType<HttpResponseMessage>();
        result.ShouldBeOfType<PirataDto>();

        Assert.NotNull(result);
        Assert.Equal("TesteComNovoCapitao", result.Nome);
        Assert.Equal("TesteComNovoCapitao", result.Capitao.Nome);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task TestPostPirataWithExistingCapitao_ReturnsPirata()
    {
        // Arrange
        var mockPirataService = MockPirataService.GetMockPirataService(null);
        var client = TestClient.GetTestClient(mockPirataService.Object);

        // Act
        var response = await client.PostAsJsonAsync("/Pirata", new PirataDto
        {
            Nome = "TesteComCapitaoExistente",
            Funcao = "TesteComCapitaoExistente",
            DataIngressoTripulacao = DateTime.Now,
            Objetivo = "TesteComCapitaoExistente",
            Capitao = new CapitaoDto
            {
                Nome = "Monkey D. Luffy"
            }
        });
        var result = await response.Content.ReadFromJsonAsync<PirataDto>();

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        response.ShouldBeOfType<HttpResponseMessage>();
        result.ShouldBeOfType<PirataDto>();

        Assert.NotNull(result);
        Assert.Equal("TesteComCapitaoExistente", result.Nome);
        Assert.Equal("Monkey D. Luffy", result.Capitao.Nome);
        Assert.Equal(2, result.Capitao.Id);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task TestPutPirata_ReturnsPirata()
    {
        // Arrange
        var mockPirataService = MockPirataService.GetMockPirataService(null);
        var client = TestClient.GetTestClient(mockPirataService.Object);

        // Act
        var response = await client.PutAsJsonAsync("/Pirata/1", new PirataDto
        {
            Nome = "TesteController",
            Funcao = "TesteController",
            DataIngressoTripulacao = DateTime.Now,
            Objetivo = "TesteController",

        });
        var result = await response.Content.ReadFromJsonAsync<PirataDto>();

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        response.ShouldBeOfType<HttpResponseMessage>();
        result.ShouldBeOfType<PirataDto>();

        Assert.NotNull(result);
        Assert.Equal("TesteController", result.Nome);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task TestPutPirata_ReturnsBadRequest()
    {
        // Arrange
        var mockPirataService = MockPirataService.GetMockPirataService(null);
        var client = TestClient.GetTestClient(mockPirataService.Object);

        // Act
        var response = await client.PutAsJsonAsync("/Pirata/0", new PirataDto
        {
            Nome = "TesteController",
            Funcao = "TesteController",
            DataIngressoTripulacao = DateTime.Now,
            Objetivo = "TesteController",

        });

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("Erro ao tentar atualizar pirata.", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task TestDeletePirata_ReturnsOk()
    {
        // Arrange
        var mockPirataService = MockPirataService.GetMockPirataService(null);
        var client = TestClient.GetTestClient(mockPirataService.Object);

        // Act
        var response = await client.DeleteAsync("/Pirata/1");
        var result = await response.Content.ReadAsStringAsync();

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Deletado", result);
    }

    [Fact]
    public async Task TestDeletePirata_ReturnsBadRequest()
    {
        // Arrange
        var mockPirataService = MockPirataService.GetMockPirataService(null);
        var client = TestClient.GetTestClient(mockPirataService.Object);

        // Act
        var response = await client.DeleteAsync("/Pirata/0");
        var result = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("Pirata não deletado", result);
    }
}