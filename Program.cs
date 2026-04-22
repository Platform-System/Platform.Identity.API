using Microsoft.EntityFrameworkCore;
using Platform.Api.Extensions;
using Platform.Identity.API.Infrastructure.Data;
using Platform.Identity.API.Infrastructure.DependencyInjection;
using Platform.Identity.API.Presentation.Grpc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityServices(builder.Configuration, typeof(Program).Assembly);
builder.Services.AddControllers();
builder.Services.AddGrpc();
builder.Services.AddPlatformAuthentication(builder.Configuration);
builder.Services.AddPlatformSwaggerJwt("Platform Identity API");

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.UsePlatformSwagger();
app.UsePlatformAuthentication();

app.MapGrpcService<IdentityIntegrationService>();
app.MapControllers();

app.Run();

