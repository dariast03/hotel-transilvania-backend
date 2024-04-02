using System.Text;
using HotelTransilvania.Context;
using HotelTransilvania.Models;
using HotelTransilvania.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<RegisterLoginContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DbCon")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IJwtService, JwtService>();

// Add EmailService
var smtpSettings = builder.Configuration.GetSection("SmtpSettings").Get<SmptSettings>();
builder.Services.AddSingleton(new EmailService(smtpSettings.SmtpServer, smtpSettings.SmtpPort, smtpSettings.SmtpUsername, smtpSettings.SmtpPassword));


// Add Swagger authentication
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Ingresa el token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
   {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
   });
});

// Route url in lowercase
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Add jwt authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });


// Add Cors
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.UseAuthorization();
app.UseCors("ConfigCors");

app.Run();
