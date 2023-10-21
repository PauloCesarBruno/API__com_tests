using APICatalogo.Context;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Filter;
using APICatalogo.Logging;
using APICatalogo.Repository;
using APICatalogo.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    // Tratamento para Desserialização ciclica:
    .AddJsonOptions(options=>
        options.JsonSerializerOptions
           .ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

// Configuração do AutoMapper:
var mappingConfig = new MapperConfiguration(mc =>
    mc.AddProfile(new MappingProfile()));

IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// Configurando a string de conexão que esta no appsettings.json
string SqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(SqlConnection));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ApiLoggingFilter>();

builder.Logging.AddProvider(new CustomLoggerProvider (new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information
}));


builder.Services.AddTransient<IMeuServico, MeuServico>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x.AllowAnyHeader().AllowAnyHeader().AllowAnyOrigin());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
