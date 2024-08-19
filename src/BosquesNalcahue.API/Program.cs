using BosquesNalcahue.API.Converters;
using BosquesNalcahue.API.Mapping;
using BosquesNalcahue.Application;
using BosquesNalcahue.Application.Entities;
using BosquesNalcahue.Application.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
{
    // QuestPDF Community License
    QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

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
        //options.AddPolicy("AngularApp", policy =>
        //{
        //    policy.WithOrigins("http://localhost:4200")
        //        .AllowAnyMethod()
        //        .AllowAnyHeader();
        //});

        options.AddPolicy("Testing", policy =>
        {
            policy.AllowAnyOrigin()
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
}

var app = builder.Build();
{

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    //app.MapIdentityApi<WebPortalUser>();

    app.UseHttpsRedirection();

    app.UseCors("Testing");

    app.UseAuthentication();

    app.UseAuthorization();

    app.UseMiddleware<ValidationMappingMiddleware>();

    app.MapControllers();

    app.Run();
}