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

namespace BinksNoSake.Tests.IntegrationTests.Controllers;
public class PirataControllerIntegrationTests
{
    private readonly WebApplicationFactory<Program> _factory;
    private HttpClient _httpClient;
    public PirataControllerIntegrationTests()
    {
        _factory = new WebApplicationFactory<Program>();
        _httpClient = _factory.CreateDefaultClient();
    }

    [Fact]
    public async Task TestGetPiratas_ReturnsSuccessStatusCode()
    {
        // Arrange
        var client = _factory.CreateClient();

        // // Act
        var response = await client.GetAsync("/Pirata");

        // // Assert
        response.Content.ReadFromJsonAsync<PirataDto>();
        
        response.EnsureSuccessStatusCode(); // Status Code 200-299


        Assert.NotNull(response.Content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}