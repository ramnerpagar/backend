using NLog.Extensions.Logging;
using NLog.Web;
using Admin_Data.DataContext;
using Microsoft.EntityFrameworkCore;
using Admin_Data.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var _authkey = builder.Configuration.GetValue<string>("JwtSettings:securitykey");

builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddAuthentication(item =>
{
    item.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
    item.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(item =>
{
    item.RequireHttpsMetadata = true;
    item.SaveToken = true;
    item.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey= true,
        IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authkey)),
        ValidateIssuer=false,
        ValidateAudience=false,
    };
});
// Add services to the container.
builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
 builder.Services.AddEndpointsApiExplorer();
 builder.Services.AddSwaggerGen();
 builder.Services.AddDbContext<AdminDBContext>(options => options.UseSqlServer(
     builder.Configuration.GetConnectionString("SQL")
     ));

var _jwtsettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(_jwtsettings);


//var authConf = new JwtSettings(builder.Configuration);

//builder.Configuration.GetSection(nameof(JwtSettings)).Bind(authConf);

var app = builder.Build();

    app.UseCors(options => 
    options.WithOrigins("http://localhost:3000")
    .AllowAnyMethod()
    .AllowAnyHeader());

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

    app.Run();

