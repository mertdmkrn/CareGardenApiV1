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

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var currentPath = Path.Combine(AppContext.BaseDirectory.Replace("bin\\Debug\\net7.0\\", ""));

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
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
        app.UseSwaggerUI();

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

        builder.Services.AddSingleton<IUserRepository, UserRepository>();
        builder.Services.AddSingleton<IConfirmationRepository, ConfirmationRepository>();
        builder.Services.AddSingleton<IServicesRepository, ServicesRepository>();
        //builder.Services.AddSingleton<IBusinessRepository, BusinessRepository>();
        //builder.Services.AddSingleton<IRatingRepository, RatingRepository>();
        //builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();
        //builder.Services.AddSingleton<IAppointmentRepository, AppointmentRepository>();
        //builder.Services.AddSingleton<IServicesRepository, ServicesRepository>();
        //builder.Services.AddSingleton<IBusinessCategoryRepository, BusinessCategoryRepository>();
        //builder.Services.AddSingleton<IBusinessGalleryRepository, BusinessGalleryRepository>();
        //builder.Services.AddSingleton<IBusinessWorkingInfoRepository, BusinessWorkingInfoRepository>();

        builder.Services.AddSingleton<IUserService, UserService>();
        builder.Services.AddSingleton<IConfirmationService, ConfirmationService>();
        builder.Services.AddSingleton<IServicesService, ServicesService>();
        //builder.Services.AddSingleton<IBusinessService, BusinessService>();
        //builder.Services.AddSingleton<IRatingService, RatingService>();
        //builder.Services.AddSingleton<ICategoryService, CategoryService>();
        //builder.Services.AddSingleton<IAppointmentService, AppointmentService>();
        //builder.Services.AddSingleton<IServicesService, ServicesService>();
        //builder.Services.AddSingleton<IBusinessCategoryService, BusinessCategoryService>();
        //builder.Services.AddSingleton<IBusinessGalleryService, BusinessGalleryService>();
        //builder.Services.AddSingleton<IBusinessWorkingInfoService, BusinessWorkingInfoService>();
    }
}