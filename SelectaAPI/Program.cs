using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Refit;
using SelectaAPI.Database;
using SelectaAPI.Integracao;
using SelectaAPI.Integracao.Interfaces;
using SelectaAPI.Integracao.Refit;
using SelectaAPI.JWT;
using SelectaAPI.Repositories.Interfaces.ProductsInterface;
using SelectaAPI.Repositories.Interfaces.UsersInterface;
using SelectaAPI.Repositories.Products;
using SelectaAPI.Repositories.Users;
using SelectaAPI.Repository;
using SelectaAPI.Repository.Interfaces;
using SelectaAPI.Services;
using SelectaAPI.Services.Interfaces;
using SelectaAPI.Services.Interfaces.ProductsInterface;
using SelectaAPI.Services.Interfaces.UsersInterface;
using SelectaAPI.Services.Products;
using SelectaAPI.Services.Users;
using System.Text;

Env.Load(); // Carrega variáveis do .env

var builder = WebApplication.CreateBuilder(args);

// ==================== Configuração de Serviços ====================

// Repositórios
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IHomeRepository, HomeRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

// Serviços
builder.Services.AddScoped<ILoginService, LoginService>();
// builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IFilesUploadAWSService, FilesUploadAWSService>();


string connectionString =
$"Server={Environment.GetEnvironmentVariable("SERVER")};" +
$"Database={Environment.GetEnvironmentVariable("DATABASE")};" +
$"User={Environment.GetEnvironmentVariable("USER")};" +
$"Password={Environment.GetEnvironmentVariable("PASSWORD")};";


/*
string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION")
                          ?? builder.Configuration.GetConnectionString("DefaultConnection");
=======
builder.Services.AddScoped<IViaCepIntegracao, ViaCepIntegracao>();

// ==================== Configuração do DB ====================
string connectionString =
    $"Server={Environment.GetEnvironmentVariable("SERVER")};" +
    $"Database={Environment.GetEnvironmentVariable("DATABASE")};" +
    $"User={Environment.GetEnvironmentVariable("USER")};" +
    $"Password={Environment.GetEnvironmentVariable("PASSWORD")};";

*/
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// ==================== Configuração AWS ====================
builder.Services.AddDefaultAWSOptions(new AWSOptions
{
    Region = Amazon.RegionEndpoint.GetBySystemName(
        Environment.GetEnvironmentVariable("AWS_REGION") ?? "sa-east-1"
    )
});

builder.Services.AddAWSService<IAmazonS3>();

// ==================== Configuração Refit ====================
builder.Services.AddRefitClient<IViaCepIntegracaoRefit>().ConfigureHttpClient(c =>
{
    c.BaseAddress = new Uri("https://viacep.com.br/");
});

// ==================== Controllers e Swagger ====================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Selecta API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Digite: Bearer {seu token JWT}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// ==================== Autenticação JWT ===============

var key = Encoding.UTF8.GetBytes(Key.Secret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
options.RequireHttpsMetadata = false;
options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
        ValidAudience = builder.Configuration["JwtConfig:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero,
    };
});

// ==================== Build e Middleware ====================
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication(); // 🔒 JWT
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "API rodando no Azure");

app.Run();
