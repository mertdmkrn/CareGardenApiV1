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
        builder.Services.AddControllers();

        builder.Services.AddMemoryCache();

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

        builder.Services.AddDbContext<CareGardenApiDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration["ConnectionStrings:AWSPostgreSQL"], x => x.UseNetTopologySuite());
        });

        builder.Services.AddTransient<ExceptionMiddleware>();
        builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

        configureInjection(builder);

        builder.Services.AddHangfire(x => x.UsePostgreSqlStorage(builder.Configuration["ConnectionStrings:AWSHangfirePostgreSQL"]));
        builder.Services.AddHangfireServer();

        var app = builder.Build();
        app.UseIpRateLimiting();

        app.UseStaticFiles();
        app.UseMiddleware<ExceptionMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseCors("corsapp");
        app.UseSwaggerUI();

        app.UseHangfireDashboard("/hangfire", new DashboardOptions()
        {
            DashboardTitle = "Hangfire Dashboard",
            Authorization = new[]{
            new HangfireCustomBasicAuthenticationFilter{
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
        builder.Services.AddSingleton<ILoggerHandler, LoggerHandler>();
        builder.Services.AddSingleton<IElasticHandler, ElasticHandler>();

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
        builder.Services.AddSingleton<IFaqRepository, FaqRepository>();
        builder.Services.AddSingleton<IWorkerRepository, WorkerRepository>();
        builder.Services.AddSingleton<IAppointmentRepository, AppointmentRepository>();
        builder.Services.AddSingleton<IDiscountRepository, DiscountRepository>();
        builder.Services.AddSingleton<ISettingRepository, SettingRepository>();

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
        builder.Services.AddSingleton<IFaqService, FaqService>();
        builder.Services.AddSingleton<IWorkerService, WorkerService>();
        builder.Services.AddSingleton<IAppointmentService, AppointmentService>();
        builder.Services.AddSingleton<IDiscountService, DiscountService>();
        builder.Services.AddSingleton<ISettingService, SettingService>();
    }
}