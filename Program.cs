using HotelTransilvania.Context;
using HotelTransilvania.Models;
using HotelTransilvania.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<RegisterLoginContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DbCon")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add EmailService
var smtpSettings = builder.Configuration.GetSection("SmtpSettings").Get<SmptSettings>();
builder.Services.AddSingleton(new EmailService(smtpSettings.SmtpServer, smtpSettings.SmtpPort, smtpSettings.SmtpUsername, smtpSettings.SmtpPassword));

// Add Cors Config
builder.Services.AddCors(options =>
{
    options.AddPolicy("ConfigCors",
           builder => builder
               .WithOrigins(
               "http://localhost:5173",
               "http://127.0.0.1:5173",
                "http://localhost:5174",
               "http://127.0.0.1:5174",
                 "http://192.168.0.12:5174",
                                  "http://192.168.0.12:5173",
               "http://localhost:4173",
               "http://127.0.0.1:4173",
               "https://tarija.upds.edu.bo"
               )
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials());
});

// Lowercase URL
builder.Services.AddRouting(options => options.LowercaseUrls = true);



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

app.UseAuthentication();

app.UseAuthorization();
app.UseCors("ConfigCors");

app.Run();
