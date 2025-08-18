using MobiMart.Api.Data;
using MobiMart.Api.Dtos;
using MobiMart.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("MobiMart");
builder.Services.AddSqlite<MobiMartContext>(connString);

var app = builder.Build();
app.MapUsersEndpoints();
app.MapBusinessesEndpoints();
app.MapInventoriesEndpoints();
app.MapDescriptionsEndpoint();

await app.MigrateDbAsync();
app.Run();
