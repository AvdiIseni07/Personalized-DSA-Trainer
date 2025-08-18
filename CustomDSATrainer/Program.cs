using Microsoft.EntityFrameworkCore;
using CustomDSATrainer.Persistance;
using System.Runtime.CompilerServices;
using CustomDSATrainer.Application;
using CustomDSATrainer.Services;

var builder = WebApplication.CreateBuilder(args);
    
builder.Services.AddDbContext<ProjectDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("WebApiDatabase")));

builder.Services.AddScoped<ProjectDbContext>();
builder.Services.AddSingleton<DatabaseService>();
builder.Services.AddSingleton<ProblemService>();
builder.Services.AddSingleton<UserOutputService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var dbService = app.Services.GetRequiredService<DatabaseService>();
dbService.init(app.Configuration);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//UserHandler.InitUser();
ActivityLogger.LogToday();

app.Run();


