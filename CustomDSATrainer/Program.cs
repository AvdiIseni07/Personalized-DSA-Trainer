using Microsoft.EntityFrameworkCore;
using CustomDSATrainer.Persistance;
using System.Runtime.CompilerServices;
using CustomDSATrainer.Application;

var builder = WebApplication.CreateBuilder(args);
    
builder.Services.AddDbContext<ProjectDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("WebApiDatabase"))); 

// Add services to the container.

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

UserHandler.InitUser();
ActivityLogger.LogToday();

app.Run();


