using fitapp.Data;
using fitapp.Services;
using Microsoft.EntityFrameworkCore;
using fitapp.Models;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<FitnessAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(" \"Server=DESKTOP-0LRF037;Database=FitnessApp;Trusted_Connection=True;TrustServerCertificate=True")));
builder.Services.AddScoped<EmailService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
if (key.Length * 8 < 256)
{
    throw new Exception("JWT key is too short. It must be at least 256 bits.");
}

var jwtSettings = new JwtSettings();
builder.Configuration.GetSection("Jwt").Bind(jwtSettings);
builder.Services.AddSingleton(jwtSettings);

//builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));


var app = builder.Build();
app.UseCors("AllowAllOrigins");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}//"Server=DESKTOP-0LRF037;Database=FitnessApp;Trusted_Connection=True;MultipleActiveResultSets=true"
//dotnet ef dbcontext scaffold "Server=DESKTOP-0LRF037;Database=FitnessApp;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
