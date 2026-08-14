using System.Text;
using BarberFlow.API.Configuration;
using BarberFlow.API.Data.Context;
using BarberFlow.API.Data.Repositories;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Repositories;
using BarberFlow.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// 1. CONFIGURAÇÃO DE INFRAESTRUTURA (BANCO DE DADOS)

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// 2. REGISTRO DE DEPENDÊNCIAS (INJEÇÃO DE DEPENDÊNCIA - D.I.)

// Repositórios (Acesso a Dados)
builder.Services.AddScoped<IEmpresaRepository, EmpresaRepository>();
builder.Services.AddScoped<IServicoRepository, ServicoRepository>();
builder.Services.AddScoped<IProfissionalRepository, ProfissionalRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IBloqueioHorarioRepository, BloqueioHorarioRepository>();
builder.Services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IHorarioFuncionamentoEmpresaRepository, HorarioFuncionamentoEmpresaRepository>();
builder.Services.AddScoped<IProfissionalServicoRepository, ProfissionalServicoRepository>();
builder.Services.AddScoped<IHorarioProfissionalRepository, HorarioProfissionalRepository>();

// Serviços (Lógica de Negócio)
builder.Services.AddScoped<EmpresaService>();
builder.Services.AddScoped<ServicoService>();
builder.Services.AddScoped<ProfissionalService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<AuthService>(); 
builder.Services.AddScoped<BloqueioHorarioService>();
builder.Services.AddScoped<AgendamentoService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<HorarioFuncionamentoEmpresaService>();
builder.Services.AddScoped<ProfissionalServicoService>();
builder.Services.AddScoped<HorarioProfissionalService>();

// 3. SEGURANÇA (AUTENTICAÇÃO E AUTORIZAÇÃO JWT)

var jwtSecret = builder.Configuration["JwtSettings:Secret"]
    ?? throw new InvalidOperationException("A chave secreta do JWT (JwtSettings:Secret) não foi configurada.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Habilitado para ambiente local
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero // Remove qualquer tolerância extra no tempo de expiração
    };
});

builder.Services.AddAuthorization();

// 4. SERVIÇOS DO FRAMEWORK (ASP.NET CORE)

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Força o JSON a interpretar qualquer data que chegue como UTC
        options.JsonSerializerOptions.Converters.Add(new JsonDateTimeConverter());
    });

// Documentação (Scalar / OpenAPI)
builder.Services.AddOpenApi();

// 5. CONSTRUÇÃO DA APLICAÇÃO (BUILD)

var app = builder.Build();

// 6. PIPELINE DE EXECUÇÃO DOS MIDDLEWARES

if (app.Environment.IsDevelopment())
{
    // Gera o arquivo JSON da especificação OpenAPI
    app.MapOpenApi();

    // Interface gráfica do Scalar API Reference
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("BarberFlow API")
               .WithTheme(ScalarTheme.Mars)
               .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();  

app.MapControllers();

app.Run();