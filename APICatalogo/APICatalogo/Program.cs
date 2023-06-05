using APICatalogo.Context;
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

// Configurando a string de conexão que esta no appsettings.json
string SqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(SqlConnection));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// O Cors é geralmente usando quando uma pagina possui um front-end ...
//app.UseCors(x => x.AllowAnyHeader().AllowAnyHeader().AllowAnyOrigin());

app.Run();
