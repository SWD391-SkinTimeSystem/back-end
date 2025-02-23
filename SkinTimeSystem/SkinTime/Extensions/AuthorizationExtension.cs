using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SkinTime.Models;
using System.Reflection;
using System.Text;

namespace SkinTime.Extensions
{
    public static class AuthorizationExtension
    {
        public static IServiceCollection ConfigurateAuthenticationMethod(this IServiceCollection services, IConfiguration configurations)
        {
            // Add authentication method with JWT.
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        //ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256Signature},
                        ValidIssuer = configurations.GetValue<string>("JWT:Issuer"),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configurations.GetValue<string>("JWT:Key")!)),
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateActor = false,
                        ValidateAudience = false,
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = async (context) =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            await context.HttpContext.Response.WriteAsJsonAsync(new ApiResponse(false, "Access denied", "Unauthorized"));
                        }
                    };
                });

            // Update swagger to allow usae of JWT in the header while testing.
            services.AddSwaggerGen(options =>
            {

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API Testing",
                    Description = "",
                    Version = "v1"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme.\n
                    Enter only your token in the text input below.\n
                    Example: '12345abcdefghi'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[] { }
                    }
                });
            });

            return services;
        }
    }
}
