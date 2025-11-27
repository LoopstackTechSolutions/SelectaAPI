using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Org.BouncyCastle.Pqc.Crypto.Crystals.Dilithium;
using Refit;
using SelectaAPI.Autenticacao;
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
using System.Text;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// ==================== CORS ====================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// ==================== Repositórios ====================
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IHomeRepository, HomeRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ISalesPersonRepository, SalesPersonRepository>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();

// ==================== Serviços ====================
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ISalesPersonService, SalesPersonService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IFilesUploadAWSService, FilesUploadAWSService>();

// ==================== Conexão com Banco ====================
string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION")
                          ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// ==================== ViaCep Refit ====================
builder.Services.AddRefitClient<IViaCepIntegracaoRefit>().ConfigureHttpClient(c =>
{
    c.BaseAddress = new Uri("https://viacep.com.br/");
});

builder.Services.AddScoped<IViaCepIntegracao, ViaCepIntegracao>();

// ==================== AWS ====================
builder.Services.AddDefaultAWSOptions(new AWSOptions
{
    Region = Amazon.RegionEndpoint.GetBySystemName(
        Environment.GetEnvironmentVariable("AWS_REGION") ?? "sa-east-1"
    )
});

builder.Services.AddAWSService<IAmazonS3>();

// ==================== Session ====================
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".Selecta.Session";
    options.IdleTimeout = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;

    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
});

// ==================== Controllers ====================
builder.Services.AddControllers();

// ==================== Swagger ====================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Selecta API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        { 
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer",
            },
            Scheme = "oauth2",
            Name = "Bearer",
            In = ParameterLocation.Header,
        },
        new List<string>()
    }
    });
});

// JWT
var key = Encoding.UTF8.GetBytes(Key.SecretKey);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// ==================== Build ====================
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllowAll");  
app.UseSession();         
app.UseAuthorization();  

app.MapControllers();

app.MapGet("/", () => "API rodando...");

app.Run();
