using DotNetEnv;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using SelectaAPI.Database;
using System;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

Env.Load();


string connectionString =
    $"Server={Environment.GetEnvironmentVariable("SERVER")};" +
    $"Database={Environment.GetEnvironmentVariable("DATABASE")};" +
    $"User={Environment.GetEnvironmentVariable("USER")};" +
    $"Password={Environment.GetEnvironmentVariable("PASSWORD")};";

Console.WriteLine("Server: " + Environment.GetEnvironmentVariable("SERVER"));
Console.WriteLine("Database: " + Environment.GetEnvironmentVariable("DATABASE"));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString,ServerVersion.AutoDetect(connectionString)));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();
