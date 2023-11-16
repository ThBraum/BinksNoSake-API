using BinksNoSake.Domain.Models;
using Microsoft.EntityFrameworkCore;
using BinksNoSake.Application.Contratos;
using BinksNoSake.Application.Services;
using BinksNoSake.Persistence.Contratos;
using BinksNoSake.Persistence.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IPirataService, PirataService>();
builder.Services.AddScoped<ICapitaoService, CapitaoService>();
builder.Services.AddScoped<IGeralPersist, GeralPersist>();
builder.Services.AddScoped<IPirataPersist, PirataPersist>();
builder.Services.AddScoped<ICapitaoPersist, CapitaoPersist>();

builder.Services.AddCors(options => //Configuração do CORS
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); //Configuração do AutoMapper

builder.Services.AddDbContext<BinksNoSakeContext>(context =>
{
    context.UseSqlite(builder.Configuration.GetConnectionString("Default"));  //("Data Source=Data\\BinksNoSake.db");
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("CorsPolicy"); //Configuração do CORS

app.MapControllers();

app.Run();
