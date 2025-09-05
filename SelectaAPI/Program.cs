using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Refit;
using SelectaAPI.Database;
using SelectaAPI.Integracao;
using SelectaAPI.Integracao.Interfaces;
using SelectaAPI.Integracao.Refit;
using SelectaAPI.Repository;
using SelectaAPI.Repository.Interfaces;
using SelectaAPI.Services;
using SelectaAPI.Services.Interfaces;

Env.Load(); // carrega o .env logo no início

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IHomeRepository, HomeRepository>();
builder.Services.AddScoped<IHomeService, HomeService>();

string connectionString =
    $"Server={Environment.GetEnvironmentVariable("SERVER")};" +
    $"Database={Environment.GetEnvironmentVariable("DATABASE")};" +
    $"User={Environment.GetEnvironmentVariable("USER")};" +
    $"Password={Environment.GetEnvironmentVariable("PASSWORD")};";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IViaCepIntegracao, ViaCepIntegracao>();

// ?? Configuração da AWS
builder.Services.AddDefaultAWSOptions(new AWSOptions
{
    Region = Amazon.RegionEndpoint.GetBySystemName(
        Environment.GetEnvironmentVariable("AWS_REGION") ?? "us-east-2"
    )
});

// ?? Injeta automaticamente o IAmazonS3 com as credenciais do .env
builder.Services.AddAWSService<IAmazonS3>();

builder.Services.AddRefitClient<IViaCepIntegracaoRefit>().ConfigureHttpClient(c =>
{
    c.BaseAddress = new Uri("https://viacep.com.br/");
});



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
