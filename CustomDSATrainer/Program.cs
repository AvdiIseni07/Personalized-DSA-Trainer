using Asp.Versioning;
using CustomDSATrainer.Application.Services;
using CustomDSATrainer.Domain.Interfaces.Services;
using CustomDSATrainer.Domain.Interfaces.UnitOfWork;
using CustomDSATrainer.Middleware;
using CustomDSATrainer.Persistance;
using CustomDSATrainer.Persistence.Repositories;
using CustomDSATrainer.Persistence.UnitOfWork;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<ProjectDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("WebApiDatabase")));

// Add services to the container.0
builder.Services.AddSingleton<DatabaseService>();
builder.Services.AddSingleton<ISeedingService, SeedingService>();
builder.Services.AddSingleton<ICurrentActiveProblemService, CurrentActiveProblemService>();
builder.Services.AddSingleton<ICurrentUserProgress, CurrentUserProgressService>();

builder.Services.AddTransient<ISubmissionService, SubmissionService>();
builder.Services.AddTransient<IAIReviewService, AIReviewService>();
builder.Services.AddTransient<ITestCaseService, TestCaseService>();
builder.Services.AddTransient<IUserSourceLinkerService, UserSourceLinkerService>();
builder.Services.AddTransient<IUserOutputService, UserOutputService>();
builder.Services.AddTransient<IPythonAIService, PythonAIService>();
builder.Services.AddTransient<IActivityLogService, ActivityLogService>();
builder.Services.AddTransient<IProblemGeneratorService, ProblemGeneratorService>();
builder.Services.AddScoped<IProblemService, ProblemService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRateLimiter(options => 
{
    options.AddFixedWindowLimiter("Fixed", limiterOptions => 
    {
        limiterOptions.PermitLimit = 1;
        limiterOptions.Window = TimeSpan.FromSeconds(10);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });    
});

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version"));
})
    .AddMvc()
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestResponseLoggingMiddleware>();


var dbService = app.Services.GetRequiredService<DatabaseService>();
dbService.init(app.Configuration);

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<ISeedingService>();
    await seeder.SeedKaggleDataset();
}

app.UseRateLimiter();

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
