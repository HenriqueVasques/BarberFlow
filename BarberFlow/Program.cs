using BarberFlow.API.Configuration;
using BarberFlow.API.Data.Context;
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
builder.Services.AddScoped<IServicoRepository, ServicoRepository>();
builder.Services.AddScoped<IProfissionalRepository, ProfissionalRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IBloqueioHorarioRepository, BloqueioHorarioRepository>();
builder.Services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();

// Serviços (Lógica de Negócio)
builder.Services.AddScoped<EmpresaService>();
builder.Services.AddScoped<ServicoService>();
builder.Services.AddScoped<ProfissionalService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<BloqueioHorarioService>();
builder.Services.AddScoped<AgendamentoService>();
builder.Services.AddScoped<ClienteService>();

// 3. SERVIÇOS DO FRAMEWORK (ASP.NET)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Isso força o JSON a interpretar qualquer data que chegue como UTC
        options.JsonSerializerOptions.Converters.Add(new JsonDateTimeConverter());
    });

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