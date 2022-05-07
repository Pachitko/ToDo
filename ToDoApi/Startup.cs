using Core.Application;
using FluentValidation.AspNetCore;
using Infrastructure;
using Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Serilog;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using ToDoApi.Models;

namespace ToDoApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructureServices(Configuration);

            services.AddApiVersioning(o =>
            {
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ReportApiVersions = true;
                o.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("v", "ver", "version", "api-version"),
                    new HeaderApiVersionReader("X-Version"));
                //o.Conventions.Controller<UserController>().HasApiVersion().Action(c => c.)
            });

            services.AddControllers(o =>
            {
                o.ReturnHttpNotAcceptable = true;
                o.SuppressAsyncSuffixInActionNames = false;
                //o.EnableEndpointRouting = false;
                o.CacheProfiles.Add("60SecondsCacheProfile", new()
                {
                    Duration = 60
                });
                //o.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
                //o.Filters.Add(new AuthorizeFilter());
                //o.ValueProviderFactories.Insert(0, new RouteBodyValueProviderFactory());
                //o.ModelBinderProviders.Insert(0, new CreateToDoItemCommandModelBinderProvider());
            })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver =
                        new CamelCasePropertyNamesContractResolver();
                })
                .AddMvcOptions(o =>
                {
                    var newtonsoftJsonOutputFormatter = o.OutputFormatters
                        .OfType<NewtonsoftJsonOutputFormatter>().FirstOrDefault();

                    if (newtonsoftJsonOutputFormatter is not null)
                    {
                        newtonsoftJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.todo.hateoas+json");
                    }
                })
                .AddXmlDataContractSerializerFormatters()
                .AddFluentValidation(config =>
                {
                    //ValidatorOptions.Global.LanguageManager.Enabled = false;
                    //ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;

                    config.AutomaticValidationEnabled = false;
                    config.DisableDataAnnotationsValidation = true;
                    config.ImplicitlyValidateRootCollectionElements = true;
                    config.RegisterValidatorsFromAssembly(typeof(Startup).Assembly);
                })
                .ConfigureApiBehaviorOptions(setup =>
                {
                    //setup.SuppressModelStateInvalidFilter = true;
                    setup.InvalidModelStateResponseFactory = ctx =>
                    {
                        var problemDetailsFactory = ctx.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                        var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(ctx.HttpContext, ctx.ModelState);

                        problemDetails.Detail = "See the errors field for details.";
                        problemDetails.Instance = ctx.HttpContext.Request.Path;

                        var actionExecutingContext = ctx as Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;

                        if (ctx.ModelState.ErrorCount > 0)// && actionExecutingContext?.ActionArguments.Count == ctx.ActionDescriptor.Parameters.Count)
                        {
                            problemDetails.Type = "https://localhost:5001/modelvalidationproblem";
                            problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                            problemDetails.Title = "One or more validation errors occured.";

                            return new UnprocessableEntityObjectResult(problemDetails)
                            {
                                ContentTypes = { "application/problem+json" }
                            };
                        }

                        problemDetails.Status = StatusCodes.Status400BadRequest;
                        problemDetails.Title = "One or more errors on input occured.";
                        return new BadRequestObjectResult(problemDetails)
                        {
                            ContentTypes = { "application/problem+json" }
                        };
                    };
                });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:9000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            var jwtSection = Configuration.GetSection("JwtOptions");
            var jwtOptions = jwtSection.Get<JwtOptions>();
            services.Configure<JwtOptions>(jwtSection);

            services.AddAuthentication(o =>
            {
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                // Override Identity default schemes
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultSignInScheme = "";
            })
                .AddJwtBearer(o =>
                {
                    SymmetricSecurityKey jwtKey = jwtOptions.GetSymmetricSecurityKey();

                    //config.Events.on
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = true;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = jwtKey,

                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,

                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,

                        RoleClaimType = ClaimTypes.Role,
                        NameClaimType = ClaimTypes.Name
                    };
                });

            services.AddStackExchangeRedisCache(o => o.Configuration = "localhost");

            services.AddAuthorization(options => ConfigureAuthorization(options));

            services.AddApplicationServices();
            services.AddToDoApiServices();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ToDo API",
                    Description = "ASP.NET Core ToDo Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Timofei Shibaev",
                        Email = "pachitko@mail.ru",
                        Url = new Uri("https://github.com/Pachitko"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under ...",
                        Url = new Uri("https://example.com/license"),
                    }
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    BearerFormat = "JWT",
                    Scheme = "Bearer",
                    Type = SecuritySchemeType.ApiKey
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
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

                options.CustomSchemaIds(type => type.FullName);
                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                options.RoutePrefix = "swagger";
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(HandleException);
            }

            app.UseHttpsRedirection();

            app.UseSerilogRequestLogging();

            var staticFileOptions = new StaticFileOptions()
            {
                OnPrepareResponse = context =>
                {
                    context.Context.Response.Headers.Add("Cache-Control", "no-cache, no-store");
                    context.Context.Response.Headers.Add("Expires", "-1");
                }
            };

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles(staticFileOptions);
            }

            app.UseStaticFiles(staticFileOptions);

            app.UseRouting();
            app.UseCors();

            app.UseAuthentication(); // Sets HttpContext.User
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
                if (env.IsDevelopment())
                {
                    //spa.UseProxyToSpaDevelopmentServer("http://localhost:9000");
                    //spa.UseReactDevelopmentServer(npmScript: "dev");
                }
            });
        }

        private static void ConfigureAuthorization(AuthorizationOptions options)
        {
            options.AddPolicy(Constants.AdminsOnly, o =>
            {
                o.RequireAuthenticatedUser();
                o.RequireRole("Admin");
                o.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
            });

            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .Build();

            //options.DefaultPolicy = fallbackPolicy;
            // todo: disabled for testing
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser() // add DenyAnonymousAuthorizationRequirement
                .Build();
        }

        private void HandleException(IApplicationBuilder app)
        {
            app.Run(async ctx =>
            {
                ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                ctx.Response.ContentType = "application/json";

                //var exceptionHandlerFeature = ctx.Features.Get<IExceptionHandlerFeature>();

                await ctx.Response.WriteAsync(new ErrorDetails()
                {
                    StatusCode = ctx.Response.StatusCode,
                    Message = "Internal Server Error."
                }.ToString());
            });
        }

        private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
        {
            var builder = new ServiceCollection()
                .AddLogging()
                .AddMvc()
                .AddNewtonsoftJson()
                .Services.BuildServiceProvider();

            return builder
                .GetRequiredService<IOptions<MvcOptions>>()
                .Value
                .InputFormatters
                .OfType<NewtonsoftJsonPatchInputFormatter>()
                .First();
        }
    }
}