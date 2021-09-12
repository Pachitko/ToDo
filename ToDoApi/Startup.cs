using Core.Application.Services;
using Infrastructure;
using Infrastructure.Options;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Security.Claims;
using ToDoApi.Models;
using Core.Application;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using FluentValidation.AspNetCore;
using Newtonsoft.Json.Serialization;
using ToDoApi.Services;
using ToDoApi.ModelBinders;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Formatters;

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
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.AddFilter(l => l == LogLevel.None);
            });

            services.AddInfrastructureServices(loggerFactory, Configuration);

            services.AddControllers(o =>
            {
                o.ReturnHttpNotAcceptable = true;
                o.SuppressAsyncSuffixInActionNames = false;
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
                        newtonsoftJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.redray.hateoas+json");
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
            //.SetCompatibilityVersion(CompatibilityVersion.Latest);

            var jwtSection = Configuration.GetSection("JwtOptions");
            var jwtOptions = jwtSection.Get<JwtOptions>();
            services.Configure<JwtOptions>(jwtSection);

            services.AddAuthentication(options =>
            {
                // Override Identity default schemes
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = "";
            })
                .AddJwtBearer(config =>
                {
                    SymmetricSecurityKey jwtKey = jwtOptions.GetSymmetricSecurityKey();

                    //config.Events.on
                    config.RequireHttpsMetadata = false;
                    config.SaveToken = true;
                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = jwtKey,

                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,

                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,

                        RoleClaimType = ClaimTypes.Role,
                        NameClaimType = ClaimTypes.NameIdentifier
                    };
                });

            services.AddAuthorization(options => ConfigureAuthorization(options));

            services.AddToDoApiServices();
            services.AddApplicationServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(HandleException);
            }

            app.UseHttpsRedirection();

            //app.UseStatusCodePages();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // Sets HttpContext.User
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void ConfigureAuthorization(AuthorizationOptions options)
        {
            options.AddPolicy("AdminsOnly", o =>
            {
                o.RequireAuthenticatedUser();
                o.RequireRole("Admin");
                o.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
            });

            options.AddPolicy("UsersOnly", o =>
            {
                o.RequireAuthenticatedUser();
                o.RequireRole("User");
                o.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
            });

            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .Build();

            //options.DefaultPolicy = fallbackPolicy;
            //options.FallbackPolicy = new AuthorizationPolicyBuilder()
            //    .RequireAuthenticatedUser() // add DenyAnonymousAuthorizationRequirement
            //    .Build();
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
    }
}