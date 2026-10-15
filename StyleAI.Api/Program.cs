using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using StyleAI.Infrastructure.Persistence;
using StyleAI.Infrastructure.Services;
using StyleAI.Core.Interfaces;
using StyleAI.Core.Data;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

// DbContext - file-based sqlite in app folder
var dbPath = Path.Combine(builder.Environment.ContentRootPath, "styleai.db");
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite($"Data Source={dbPath}"));

// DI for core services
builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();

// register domain services
builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddScoped<IStyleService, StyleService>();

// Style catalog - point to the prebuilt styles JSON we added
var stylesJson = Path.Combine(builder.Environment.ContentRootPath, "python_ml", "styles.json");
if (File.Exists(stylesJson))
{
    builder.Services.AddSingleton<IStyleCatalog>(new StyleCatalog(stylesJson));
}

// HttpClient for Python ML microservice
var pythonBase = builder.Configuration["PythonMl:BaseUrl"] ?? "http://localhost:8000";
builder.Services.AddHttpClient<IPythonMlClient, PythonMlClient>(c => c.BaseAddress = new Uri(pythonBase));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// migrate db
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.MapControllers();

app.Run();
