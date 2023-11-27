using BinksNoSake.Domain.Models;
using Microsoft.EntityFrameworkCore;
using BinksNoSake.Application.Contratos;
using BinksNoSake.Application.Services;
using BinksNoSake.Persistence.Contratos;
using BinksNoSake.Persistence.Persistence;
using System.Text.Json.Serialization;
using BinksNoSake.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IPirataService, PirataService>();
builder.Services.AddScoped<ICapitaoService, CapitaoService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<ICapitaoPersist, CapitaoPersist>();
builder.Services.AddScoped<IPirataPersist, PirataPersist>();
builder.Services.AddScoped<IGeralPersist, GeralPersist>();
builder.Services.AddScoped<IAccountPersist, AccountPersist>();

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

builder.Services.AddIdentity<Account, Role>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
})
.AddRoles<Role>()
.AddRoleManager<RoleManager<Role>>()
.AddSignInManager<SignInManager<Account>>()
.AddRoleValidator<RoleValidator<Role>>()
.AddEntityFrameworkStores<BinksNoSakeContext>() 
.AddDefaultTokenProviders();


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; //Evita o erro de referência circular
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); //Converte os enums para string
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "ProEventos.API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header. Insira seu token.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http, 
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

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
