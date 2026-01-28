using Marketplace.API.Extensions;
using Marketplace.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()
               .Build());
});

// Add services to the container.
var configuration = builder.Configuration;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAppServices(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (Convert.ToBoolean(configuration["EnableSwagger"]))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseMiddleware<ApiKeyMiddleware>();

app.MapControllers();

app.Run();
