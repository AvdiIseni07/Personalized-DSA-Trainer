using CustomDSATrainer.Application;
using CustomDSATrainer.Application.Services;
using CustomDSATrainer.Domain.Interfaces.Repositories;
using CustomDSATrainer.Domain.Interfaces.Services;
using CustomDSATrainer.Middleware;
using CustomDSATrainer.Persistance;
using CustomDSATrainer.Persistance.Repositories;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<ProjectDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("WebApiDatabase")));

// Add services to the container.
builder.Services.AddSingleton<DatabaseService>();
builder.Services.AddSingleton<ISeedingService, SeedingService>();
builder.Services.AddSingleton<ICurrentActiveProblemService, CurrentActiveProblemService>();

builder.Services.AddTransient<ISubmissionService, SubmissionService>();
builder.Services.AddTransient<IAIReviewService, AIReviewService>();
builder.Services.AddTransient<ITestCaseService, TestCaseService>();
builder.Services.AddTransient<IUserSourceLinkerService, UserSourceLinkerService>();
builder.Services.AddTransient<IUserOutputService, UserOutputService>();
builder.Services.AddTransient<IPythonAIService, PythonAIService>();
builder.Services.AddTransient<IActivityLogService, ActivityLogService>();
builder.Services.AddTransient<IProblemGeneratorService, ProblemGeneratorService>();
builder.Services.AddScoped<IProblemService, ProblemService>();

builder.Services.AddScoped<IProblemRepository, ProblemRespository>();
builder.Services.AddScoped<ISubmissionRepository, SubmissionRepository>();
builder.Services.AddScoped<ITestCaseRepository, TestCaseRepository>();
builder.Services.AddScoped<IAIReviewRepository, AIReviewRepository>();
builder.Services.AddScoped<IUserProgressRepository, UserProgressRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

var dbService = app.Services.GetRequiredService<DatabaseService>();
dbService.init(app.Configuration);

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<ISeedingService>();
    await seeder.SeedKaggleDataset();
}

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
//var activityLogger = app.Services.GetRequiredService<ActivityLogService>();
//activityLogger.LogToday();

app.Run();


