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
