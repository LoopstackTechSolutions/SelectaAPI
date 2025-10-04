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

Env.Load(); // carrega o .env logo no in�cio

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IHomeRepository, HomeRepository>();
builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IFilesUploadAWSService, FilesUploadAWSService>();


string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION")
                          ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


builder.Services.AddScoped<IViaCepIntegracao, ViaCepIntegracao>();

// ?? Configura��o da AWS
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

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "API rodando no Azure");
app.Run();

