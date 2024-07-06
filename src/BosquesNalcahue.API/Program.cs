using BosquesNalcahue.API.Converters;
using BosquesNalcahue.API.Mapping;
using BosquesNalcahue.Application;
using BosquesNalcahue.Application.Entities;
using BosquesNalcahue.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// MongoDb Configuration
builder.Services.Configure<MongoDbOptions>(builder.Configuration.GetSection(nameof(MongoDbOptions)));
builder.Services.AddSingleton<IMongoDbOptions>
                    (sp => sp.GetRequiredService<IOptions<MongoDbOptions>>().Value);

// Identity Configuration
var identityDbOptions = builder.Configuration.GetSection(MongoIdentityOptions.OptionsName).Get<MongoIdentityOptions>();

builder.Services.AddDbContext<WebPortalDbContext>(options =>
{
    options.UseMongoDB(identityDbOptions?.Server ?? "", identityDbOptions?.Database ?? "");   
});

builder.Services
    .AddIdentityApiEndpoints<WebPortalUser>()
    .AddEntityFrameworkStores<WebPortalDbContext>();

builder.Services.AddReportsApp();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.Converters.Add(new ReportConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapIdentityApi<WebPortalUser>();

app.UseHttpsRedirection();

app.UseCors("AngularApp");

app.UseAuthorization();

app.UseMiddleware<ValidationMappingMiddleware>();

app.MapControllers();

app.Run();
