using BarberFlow.API.Configuration;
using BarberFlow.API.Data.Context;
using BarberFlow.API.Data.Repositories;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Interfaces.IRepository;
using BarberFlow.API.Interfaces.IServices;
using BarberFlow.API.Repositories;
using BarberFlow.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. INFRAESTRUTURA & BANCO DE DADOS

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// 2. INJEÇÃO DE DEPENDÊNCIA (D.I.)

// Infraestrutura / Transações
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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

// Serviços (Regras de Negócio - Mapeados para suas respectivas Interfaces)
builder.Services.AddScoped<IEmpresaService, EmpresaService>();
builder.Services.AddScoped<IServicoService, ServicoService>();
builder.Services.AddScoped<IProfissionalService, ProfissionalService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBloqueioHorarioService, BloqueioHorarioService>();
builder.Services.AddScoped<IAgendamentoService, AgendamentoService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IHorarioFuncionamentoEmpresaService, HorarioFuncionamentoEmpresaService>();
builder.Services.AddScoped<IProfissionalServicoService, ProfissionalServicoService>();
builder.Services.AddScoped<IHorarioProfissionalService, HorarioProfissionalService>();
// 3. SEGURANÇA (CORS, AUTENTICAÇÃO E AUTORIZAÇÃO JWT)

// Configuração do CORS
var allowedOrigins = builder.Configuration["CorsSettings:AllowedOrigins"] ?? "http://localhost:5173";

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configuração do JWT
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
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// 4. SERVIÇOS DO FRAMEWORK E DOCUMENTAÇÃO

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Converter global para interpretar datas recebidas como UTC
        options.JsonSerializerOptions.Converters.Add(new JsonDateTimeConverter());
    });

// Configuração do OpenAPI com suporte a autenticação JWT no Scalar
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        var securityScheme = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Cole seu token JWT abaixo:"
        };

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes.Add("Bearer", securityScheme);

        return Task.CompletedTask;
    });
});

// 5. CONSTRUÇÃO E PIPELINE DE MIDDLEWARES

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("BarberFlow API")
               .WithTheme(ScalarTheme.Mars)
               .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();