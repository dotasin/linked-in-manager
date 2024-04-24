using LinkedInManager.Data;
using LinkedInManager.Logger;
using LinkedInManager.Service;
using LinkedInManager.Settings;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var appSettings = builder.Configuration.Get<AppSettings>()!;

// Configure logging with Serilog
builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
});

builder.Services.AddSingleton(appSettings);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:5213")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddTransient<IEmailService, EmailService>(); 
builder.Services.AddTransient<ISearchService, SearchService>();
builder.Services.AddTransient<ITechnologyService, TechnologyService>();
builder.Services.AddTransient<ILinkedInPeopleService, LinkedInPeopleService>();
builder.Services.AddTransient<ICompanyEmployerService, CompanyEmployerService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddAuthorization();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(appSettings.DbSettings.GetSqlConnectionString());
});

var app = builder.Build();

app.UseMiddleware<RequestResponseLoggingMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowSpecificOrigin");

app.MapControllers();

app.Run();