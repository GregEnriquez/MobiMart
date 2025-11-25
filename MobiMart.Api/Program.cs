using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MobiMart.Api.Data;
using MobiMart.Api.Endpoints;
using MobiMart.Api.Services;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("MobiMart");
builder.Services.AddSqlite<MobiMartContext>(connString);
builder.Services.AddControllers();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["AppSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["AppSettings:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)),
            ValidateIssuerSigningKey = true
        };
    });


var app = builder.Build();
app.MapUsersEndpoints().RequireAuthorization();
app.MapBusinessEndpoints().RequireAuthorization();
app.MapSalesEndpoints().RequireAuthorization();
app.MapInventoryEndpoints();
app.MapSupplyChainEndpoints();

app.MapControllers();

await app.MigrateDbAsync();
app.Run();
 