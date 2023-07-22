using CareGardenApiV1.Handler.Abstract;
using TokenHandler = CareGardenApiV1.Handler.Concrete.TokenHandler;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Repository.Concrete;
using CareGardenApiV1.Service.Abstract;
using CareGardenApiV1.Service.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using CareGardenApiV1.Model;
using Microsoft.Extensions.Configuration;
using CareGardenApiV1.Handler.Model;
using CareGardenApiV1.Handler.Concrete;
using System.Text.Json.Serialization;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Serilog;
using Serilog.Formatting.Compact;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var currentPath = Path.Combine(AppContext.BaseDirectory.Replace("bin\\Debug\\net7.0\\", ""));

        string path = Path.Combine(Directory.GetCurrentDirectory(), @"StaticFiles\UploadedFiles");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Logger(lc => lc
                .MinimumLevel.Warning()
                .WriteTo.File(new CompactJsonFormatter(), "StaticFiles/Logs/log.json", rollingInterval: RollingInterval.Day, flushToDiskInterval: TimeSpan.Zero))
            .WriteTo.Logger(lc => lc
                .MinimumLevel.Error()
                .WriteTo.File(new CompactJsonFormatter(), "StaticFiles/Logs/error.json", rollingInterval: RollingInterval.Day, flushToDiskInterval: TimeSpan.Zero))
            .CreateLogger();

        builder.Host.UseSerilog();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
        {
            builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
        }));

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        builder.Services.AddSwaggerGen(c =>
        {
            var filePath = Path.Combine(currentPath + "CareGardenApiV1.xml");
            c.IncludeXmlComments(filePath);

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                    new string[] { }
                }
            });
        });

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
                options.TokenValidationParameters = new()
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecurityKey"])),
                    ClockSkew = TimeSpan.Zero
                }
        );

        builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

        configureInjection(builder);

        var app = builder.Build();

        app.UseSwagger();
        app.UseCors("corsapp");
        app.UseSwaggerUI();

        app.UseStaticFiles(new StaticFileOptions()
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"StaticFiles")),
            RequestPath = new PathString("/staticfiles")
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    private static void configureInjection(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ITokenHandler, TokenHandler>();
        builder.Services.AddSingleton<IMailHandler, MailHandler>();
        builder.Services.AddSingleton<ISmsHandler, SmsHandler>();
        builder.Services.AddSingleton<IFileHandler, FileHandler>();
        builder.Services.AddSingleton<ILoggerHandler, LoggerHandler>();

        builder.Services.AddSingleton<IBusinessRepository, BusinessRepository>();
        builder.Services.AddSingleton<IBusinessGalleryRepository, BusinessGalleryRepository>();
        builder.Services.AddSingleton<IBusinessPropertiesRepository, BusinessPropertiesRepository>();
        builder.Services.AddSingleton<IBusinessServicesRepository, BusinessServicesRepository>();
        builder.Services.AddSingleton<IBusinessWorkingInfoRepository, BusinessWorkingInfoRepository>();
        builder.Services.AddSingleton<ICampaignRepository, CampaignRepository>();
        builder.Services.AddSingleton<ICommentRepository, CommentRepository>();
        builder.Services.AddSingleton<IComplainRepository, ComplainRepository>();
        builder.Services.AddSingleton<IConfirmationRepository, ConfirmationRepository>();
        builder.Services.AddSingleton<IFavoriteRepository, FavoriteRepository>();
        builder.Services.AddSingleton<IPaymentInfoRepository, PaymentInfoRepository>();
        builder.Services.AddSingleton<IServicesRepository, ServicesRepository>();
        builder.Services.AddSingleton<IUserRepository, UserRepository>();

        builder.Services.AddSingleton<IBusinessService, BusinessService>();
        builder.Services.AddSingleton<IBusinessGalleryService, BusinessGalleryService>();
        builder.Services.AddSingleton<IBusinessPropertiesService, BusinessPropertiesService>();
        builder.Services.AddSingleton<IBusinessServicesService, BusinessServicesService>();
        builder.Services.AddSingleton<IBusinessWorkingInfoService, BusinessWorkingInfoService>();
        builder.Services.AddSingleton<ICampaignService, CampaignService>();
        builder.Services.AddSingleton<ICommentService, CommentService>();
        builder.Services.AddSingleton<IComplainService, ComplainService>();
        builder.Services.AddSingleton<IConfirmationService, ConfirmationService>();
        builder.Services.AddSingleton<IFavoriteService, FavoriteService>();
        builder.Services.AddSingleton<IPaymentInfoService, PaymentInfoService>();
        builder.Services.AddSingleton<IServicesService, ServicesService>();
        builder.Services.AddSingleton<IUserService, UserService>();
    }
}