using BarberFlow.API.Data;
using BarberFlow.API.Data.Repositories;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// 1. CONFIGURAÇÃO DE INFRAESTRUTURA (BANCO)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// 2. REGISTRO DE DEPENDÊNCIAS (D.I.)

// Repositórios (Acesso a Dados)
builder.Services.AddScoped<IEmpresaRepository, EmpresaRepository>();

// Serviços (Lógica de Negócio)
builder.Services.AddScoped<EmpresaService>();

// 3. SERVIÇOS DO FRAMEWORK (ASP.NET)
builder.Services.AddControllers();

// Documentação (Swagger/OpenApi)
builder.Services.AddOpenApi();

// 4. CONSTRUÇÃO DO APP (BUILD)
var app = builder.Build();

// 5. MIDDLEWARES (PIPELINE DE EXECUÇÃO)
if (app.Environment.IsDevelopment())
{
    // Gera o arquivo JSON da especificação
    app.MapOpenApi();

    // Cria a interface visual do Scalar
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("BarberFlow API")
               .WithTheme(ScalarTheme.Mars) // Você pode mudar o tema aqui!
               .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();