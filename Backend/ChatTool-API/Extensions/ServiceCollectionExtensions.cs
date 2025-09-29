using ChatTool.API.Interfaces;
using ChatTool.API.Managers;
using ChatTool.Database;
using ChatTool.Mapper;
using ChatTool.Models.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace ChatTool.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddChatToolServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            // 🔑 Add JWT Bearer Auth to Swagger
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter 'Bearer' [space] and then your valid token.\n\nExample: \"Bearer eyJhbGciOiJI...\""
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        // Add CORS policy for localhost only
        services.AddCors(options =>
        {
            options.AddPolicy("LocalhostOnly", policy =>
            {
                policy.WithOrigins("http://localhost", "https://localhost")
                      .AllowAnyHeader()
                      .AllowAnyMethod();
                policy.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
            });
        });

        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<UserProfile>();
            cfg.AddProfile<ChatProfile>();
            cfg.AddProfile<MessageProfile>();
        });

        services.Configure<AppSettings>(configuration);

        services.AddDbContext<DBContext>(options =>
        {
            //options.UseInMemoryDatabase("ChatToolDB");
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), builder =>
            {
                builder.MigrationsAssembly("ChatTool.API");
            });
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
        });

        services.AddScoped<ITokenManager, TokenManager>();
        services.AddScoped<IAuthManager, AuthManager>();
        services.AddScoped<IUserManager, UserManager>();
        services.AddScoped<IChatManager, ChatManager>();
        services.AddScoped<IMessageManager, MessageManager>();

        return services;
    }
}
