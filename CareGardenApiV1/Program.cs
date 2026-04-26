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
using CareGardenApiV1.Handler.Model;
using CareGardenApiV1.Handler.Concrete;
using Serilog;
using Serilog.Formatting.Compact;
using Hangfire;
using Hangfire.PostgreSql;
using HangfireBasicAuthenticationFilter;
using CareGardenApiV1.Hangfire;
using CareGardenApiV1.Middleware;
using CareGardenApiV1.Helpers;
using AspNetCoreRateLimit;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using CareGardenApiV1.Repository;
using System.Text.Json.Serialization;
using Nominatim.API.Interfaces;
using Nominatim.API.Web;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Logger(lc => lc
                .MinimumLevel.Warning()
                .WriteTo.File(new CompactJsonFormatter(), "StaticFiles/Logs/log.json", rollingInterval: RollingInterval.Day, flushToDiskInterval: TimeSpan.Zero))
            .WriteTo.Logger(lc => lc
                .MinimumLevel.Error()
                .WriteTo.File(new CompactJsonFormatter(), "StaticFiles/Logs/error.json", rollingInterval: RollingInterval.Day, flushToDiskInterval: TimeSpan.Zero))
            .CreateLogger();

        builder.Host.UseSerilog();
        builder.Services.AddElasticSearch(builder.Configuration);
        builder.Services.AddOptions();
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

        builder.Services.AddMemoryCache();
        builder.Services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
        });

        builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
        builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
        builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
        {
            builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
        }));

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        builder.Services.AddSwaggerGen(c =>
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

            c.SwaggerDoc("v1", new OpenApiInfo { Title = "CareGarden API", Version = "v1" });

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

        builder.Services.AddTransient<ExceptionMiddleware>();
        builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

        configureInjection(builder);

        builder.Services.AddDbContext<CareGardenApiDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration["ConnectionStrings:NeonPostgreSQL"], x => x.UseNetTopologySuite());
        });

        builder.Services.AddHangfire(x => x.UsePostgreSqlStorage(builder.Configuration["ConnectionStrings:NeonHangfirePostgreSQL"]));
        builder.Services.AddHangfireServer();
        builder.Services.AddHttpClient();

        var app = builder.Build();

        app.UseSwagger();
        app.UseIpRateLimiting();
        app.UseResponseCompression();

        app.UseStaticFiles();
        app.UseMiddleware<ExceptionMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCors("corsapp");
        app.UseSwaggerUI();

        app.UseHangfireDashboard("/hangfire", new DashboardOptions()
        {
            DashboardTitle = "Hangfire Dashboard",
            Authorization = new[] {
                new HangfireCustomBasicAuthenticationFilter {
                    User = builder.Configuration["HangfireSettings:UserName"],
                    Pass = builder.Configuration["HangfireSettings:Password"]
                },
            }
        });

        app.UseHangfireServer(new BackgroundJobServerOptions());
        HangfireJobScheduler.ScheduleRecurringJob();

        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"StaticFiles\UploadedFiles");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        app.MapControllers();
        app.Run();
    }

    private static void configureInjection(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ITokenHandler, TokenHandler>();
        builder.Services.AddSingleton<IMailHandler, MailHandler>();
        builder.Services.AddSingleton<ISmsHandler, SmsHandler>();
        builder.Services.AddSingleton<IFileHandler, FileHandler>();
        builder.Services.AddSingleton<IOpenAIHandler, OpenAIHandler>();
        builder.Services.AddSingleton<INominatimWebInterface, NominatimWebInterface>();

        builder.Services.AddScoped<ILoggerHandler, LoggerHandler>();
        builder.Services.AddScoped<IElasticHandler, ElasticHandler>();

        builder.Services.AddScoped<IBusinessRepository, BusinessRepository>();
        builder.Services.AddScoped<IBusinessUserRepository, BusinessUserRepository>();
        builder.Services.AddScoped<IBusinessGalleryRepository, BusinessGalleryRepository>();
        builder.Services.AddScoped<IBusinessPropertiesRepository, BusinessPropertiesRepository>();
        builder.Services.AddScoped<IBusinessServicesRepository, BusinessServicesRepository>();
        builder.Services.AddScoped<IBusinessWorkingInfoRepository, BusinessWorkingInfoRepository>();
        builder.Services.AddScoped<ICampaignRepository, CampaignRepository>();
        builder.Services.AddScoped<ICommentRepository, CommentRepository>();
        builder.Services.AddScoped<IComplainRepository, ComplainRepository>();
        builder.Services.AddScoped<IConfirmationRepository, ConfirmationRepository>();
        builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
        builder.Services.AddScoped<IPaymentInfoRepository, PaymentInfoRepository>();
        builder.Services.AddScoped<IServicesRepository, ServicesRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IFaqRepository, FaqRepository>();
        builder.Services.AddScoped<IWorkerRepository, WorkerRepository>();
        builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
        builder.Services.AddScoped<ISettingRepository, SettingRepository>();
        builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
        builder.Services.AddScoped<IAppointmentDetailRepository, AppointmentDetailRepository>();
        builder.Services.AddScoped<IWorkerServicePriceRepository, WorkerServicePriceRepository>();
        builder.Services.AddScoped<IResetLinkRepository, ResetLinkRepository>();
        builder.Services.AddScoped<IBusinessCustomerRepository, BusinessCustomerRepository>();
        builder.Services.AddScoped<IBusinessPaymentRepository, BusinessPaymentRepository>();
        builder.Services.AddScoped<IBusinessAdminRepository, BusinessAdminRepository>();

        builder.Services.AddScoped<IBusinessService, BusinessService>();
        builder.Services.AddScoped<IBusinessUserService, BusinessUserService>();
        builder.Services.AddScoped<IBusinessGalleryService, BusinessGalleryService>();
        builder.Services.AddScoped<IBusinessPropertiesService, BusinessPropertiesService>();
        builder.Services.AddScoped<IBusinessServicesService, BusinessServicesService>();
        builder.Services.AddScoped<IBusinessWorkingInfoService, BusinessWorkingInfoService>();
        builder.Services.AddScoped<ICampaignService, CampaignService>();
        builder.Services.AddScoped<ICommentService, CommentService>();
        builder.Services.AddScoped<IComplainService, ComplainService>();
        builder.Services.AddScoped<IConfirmationService, ConfirmationService>();
        builder.Services.AddScoped<IFavoriteService, FavoriteService>();
        builder.Services.AddScoped<IPaymentInfoService, PaymentInfoService>();
        builder.Services.AddScoped<IServicesService, ServicesService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IFaqService, FaqService>();
        builder.Services.AddScoped<IWorkerService, WorkerService>();
        builder.Services.AddScoped<IAppointmentService, AppointmentService>();
        builder.Services.AddScoped<IDiscountService, DiscountService>();
        builder.Services.AddScoped<ISettingService, SettingService>();
        builder.Services.AddScoped<INotificationService, NotificationService>();
        builder.Services.AddScoped<IAppointmentDetailService, AppointmentDetailService>();
        builder.Services.AddScoped<IWorkerServicePriceService, WorkerServicePriceService>();
        builder.Services.AddScoped<IResetLinkService, ResetLinkService>();
        builder.Services.AddScoped<IBusinessCustomerService, BusinessCustomerService>();
        builder.Services.AddScoped<IBusinessPaymentService, BusinessPaymentService>();
        builder.Services.AddScoped<IBusinessAdminService, BusinessAdminService>();
    }
}