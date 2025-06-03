using Hangfire;
using RaritetBooks.API.Common;
using RaritetBooks.API.Middleware;
using RaritetBooks.Application;
using RaritetBooks.Infrastructure;
using RaritetBooks.Infrastructure.Jobs;
using RaritetBooks.Infrastructure.Kafka;
using RaritetBooks.Infrastructure.TelegramBot;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .WriteTo.Seq(builder.Configuration.GetSection("Seq").Value
                 ?? throw new ApplicationException("Seq configuration is wrong"))
    .CreateLogger();

builder.Services.AddFluentValidationAutoValidation(configuration =>
    configuration.OverrideDefaultResultFactoryWith<CustomResultFactory>());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSerilog();

builder.Services.AddSwagger();
builder.Services.AddSwaggerGen(config =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    config.IncludeXmlComments(xmlPath);
});

builder.Services.AddApiVersioning();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddInfrastructureKafka(builder.Configuration)
    .AddInfrastructureTelegram(builder.Configuration);

builder.Services.AddAuth(builder.Configuration);

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(
        policy =>
            policy
                .WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()));

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseSerilogRequestLogging();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseApiVersioning();
app.MapControllers();

app.UseHangfireDashboard();
app.MapHangfireDashboard();
HangfireWorker.StartRecurringJobs();

app.Run();