using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Glasses.Data;
using Microsoft.AspNetCore.Builder;
using Glasses.Repository;
using Glasses.IRepository;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System.Configuration;
using Microsoft.AspNetCore.Identity;
using Glasses.Model;
using AutoMapper;
using Glasses.Configurations;
using Glasses;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<GlassesContext>(options =>
//    options.UseSqlite(builder.Configuration.GetConnectionString("GlassesContext") ?? throw new InvalidOperationException("Connection string 'GlassesContext' not found.")));

//builder.Services.AddDbContext<GlassesContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"))
//    );

String mySqlConnectionStr = builder.Configuration.GetConnectionString("DbConnection");
builder.Services.AddDbContext<GlassesContext>(options => options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr)));

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddControllers().AddNewtonsoftJson(
    op=>op.SerializerSettings.ReferenceLoopHandling=
    Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JwtAuthorization",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"

        });

        c.AddSecurityRequirement( new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference =new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new String[]{}
            }

        });


    });

builder.Services.AddTransient<iUnitOfWorks, UnitOfWorks>();
builder.Services.AddScoped<IProductService, ProductService>();


builder.Services.AddSingleton(Serilog.Log.Logger);
builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();
builder.Services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

// Logging
builder.Services.AddLogging();

builder.Services.AddAutoMapper(typeof(MapperInitializer));


//string Jwtissuerconstr = builder.Configuration.GetConnectionString("Jwt:Issuer");
//string Jwtaudienceconstr = builder.Configuration.GetConnectionString("Jwt:Audience");

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]))


        };
    });

//builder.Services.ConfigureIdentity();

var app = builder.Build();

Log.Logger = (Serilog.ILogger)new LoggerConfiguration()
    .WriteTo.File(
    path: "/Users/armah/Desktop/HDigital/Glasses/Glasses/logs/Logss.txt",
    //path: "/Users/armah/Desktop/H Digital/Glasses//logs//Log-.txt",
    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}[{Level:u3}]{Message:lj}{NewLine}{Exception}",
    rollingInterval: RollingInterval.Day,
    restrictedToMinimumLevel: LogEventLevel.Information).CreateLogger();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
}


app.UseAuthentication();


app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern:"{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllers();

});
    

app.Run();

