using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Refit;
using SelectaAPI.Database;
using SelectaAPI.Integracao;
using SelectaAPI.Integracao.Interfaces;
using SelectaAPI.Integracao.Refit;
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

Env.Load(); // carrega o .env logo no início

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IHomeRepository, HomeRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
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

*/
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IViaCepIntegracao, ViaCepIntegracao>();

// ?? Configuração da AWS
builder.Services.AddDefaultAWSOptions(new AWSOptions
{
    Region = Amazon.RegionEndpoint.GetBySystemName(
        Environment.GetEnvironmentVariable("AWS_REGION") ?? "sa-east-1"
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

