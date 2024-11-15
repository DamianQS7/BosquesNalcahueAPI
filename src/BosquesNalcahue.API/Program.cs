using BosquesNalcahue.API.Auth;
using BosquesNalcahue.API.Converters;
using BosquesNalcahue.API.Mapping;
using BosquesNalcahue.Application;
using BosquesNalcahue.Application.Entities;
using BosquesNalcahue.Application.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
{
    // QuestPDF Community License
    QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

    // Logging configuration
    builder.Services.AddHttpLogging(options => 
    {
        options.LoggingFields = HttpLoggingFields.RequestMethod |
                                HttpLoggingFields.RequestHeaders |
                                HttpLoggingFields.Response;
    });

    // AzureBlobStorage Configuration
    builder.Services.Configure<BlobStorageConfig>(builder.Configuration.GetSection(nameof(BlobStorageConfig)));

    // MongoDb Configuration
    builder.Services.Configure<MongoDbOptions>(builder.Configuration.GetSection(nameof(MongoDbOptions)));
    builder.Services.AddSingleton<IMongoDbOptions>
                        (sp => sp.GetRequiredService<IOptions<MongoDbOptions>>().Value);

    // JwtService Configuration
    builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));
    var jwtSettings = builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();

    // Identity Configuration
    var identityDbOptions = builder.Configuration.GetSection("MongoDbIdentity").Get<MongoIdentityOptions>();

    builder.Services.AddDbContext<WebPortalDbContext>(options =>
    {
        options.UseMongoDB(identityDbOptions?.Server ?? "", identityDbOptions?.Database ?? "");
    });

    builder.Services
        .AddIdentity<WebPortalUser, IdentityRole>()
        .AddEntityFrameworkStores<WebPortalDbContext>();

    // Configure Authentication
    builder.Services.AddScoped<ApiKeyAuthFilter>();

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings?.ValidIssuer,
            ValidAudience = jwtSettings?.ValidAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.SecurityKey ?? ""))
        };
    });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("Admin", policy => policy.RequireClaim("IsAdmin", "True"));
    });

    builder.Services.AddReportsApp(builder.Environment.ContentRootPath);

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("Testing", policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });

        options.AddPolicy("AngularApp", policy =>
        {
            policy.WithOrigins("https://bosquesnalcahue-webportal.pages.dev")
                .AllowAnyMethod()
                .AllowAnyHeader();
        });

        options.AddPolicy("MobileClient", policy =>
        {
            policy.AllowAnyOrigin()
                .WithMethods("POST")
                .WithHeaders("api-key");
        });
    });

    builder.Services.AddControllers().AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new ReportConverter());
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();
{

    app.UseSwagger();
    app.UseSwaggerUI();

    //app.MapIdentityApi<WebPortalUser>();
    app.UseHttpLogging();

    app.UseHttpsRedirection();

    app.UseCors("Testing");

    app.UseCors("AngularApp");

    app.UseCors("MobileClient");

    app.UseAuthentication();

    app.UseAuthorization();

    app.UseMiddleware<ValidationMappingMiddleware>();

    app.MapControllers();

    app.Run();
}
