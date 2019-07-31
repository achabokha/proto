using AspNet.Security.OpenIdConnect.Primitives;
using Swashbuckle.AspNetCore.Swagger;
using AutoMapper;
using Embily.Gateways;
using Embily.Models;
using Embily.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System;
using EmbilyServices.Hubs;
using System.Diagnostics;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.IO;

namespace EmbilyServices
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var env = Configuration["ASPNETCORE_ENVIRONMENT"];

            services.AddMemoryCache();

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AddPageRoute("/Index", "{*url}");
                });

            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    // handle loops correctly
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

                    // use standard name conversion of properties
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                    //Default DateFormat
                    options.SerializerSettings.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";

                    options.SerializerSettings.Formatting = Formatting.Indented;
                });

            // the Angular build puts files into dist folder (ng build)
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddAutoMapper();

            services.AddSignalR();

            services.AddSwaggerGen(c =>
            {
                //c.SwaggerDoc("v2", new Info { Title = "Embily API", Version = "v2-beta1", Contact = { Email="service@embily.com", Name="Embily Services" }, Description="Embily Platform API", License = { Url= "https://services.embily.com/legal/section/terms-of-use" }, TermsOfService = "Terms of Use" });
                c.SwaggerDoc("v2", new Info { Title = "Embily API", Version = "v2-beta1", Description = "Embily Platform API", TermsOfService = "https://services.embily.com/legal/section/terms-of-use", Contact = new Contact { Name = "Service and Support", Email = "service@embily.com", Url = "https://embily.com/contact-us/" } });
                c.SwaggerDoc("v1", new Info { Title = "Embily API", Version = "v1" });
                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

                    var docs = methodInfo.DeclaringType
                        .GetCustomAttributes(true)
                        .OfType<SwaggerTagAttribute>();

                    return docs.Any();
                });

                var filePath = Path.Combine(Env.WebRootPath, "EmbilyServices.xml");
                c.IncludeXmlComments(filePath);

                c.DescribeAllEnumsAsStrings();
                c.OperationFilter<AuthResponsesOperationFilter>();

                c.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Type = "oauth2",
                    Flow = "password",
                    TokenUrl = "/connect/token",
                    //TokenUrl = "https://services.embily.com/connect/token",
                    Description = "Embily API v2 uses OAuth2 Authentication",
                    //Scopes = new Dictionary<string, string>
                    //{
                    //    { "readAccess", "Access read operations" },
                    //    { "writeAccess", "Access write operations" }
                    //}
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    //{ "oauth2", new[] { "readAccess", "writeAccess" } }
                    { "oauth2", new[] { "" } }
                });

                //c.DocumentFilter<SwaggerTagFilter>();
            });

            //var cannectiionString, check if Mac or Windows -- 
            var connectionString = Environment.OSVersion.Platform == PlatformID.Unix ? "EmbilyDatabaseUnix" : "EmbilyDatabase";
            var connection = Configuration.GetConnectionString(connectionString);
            services.AddDbContext<EmbilyDbContext>(options =>
            {
                options.UseSqlServer(connection);
                options.UseOpenIddict();
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<EmbilyDbContext>()
            .AddDefaultTokenProviders();

            // Configure Identity to use the same JWT claims as OpenIddict instead
            // of the legacy WS-Federation claims it uses by default (ClaimTypes),
            // which saves you from doing the mapping in your authorization controller.
            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            });

            // Register the OpenIddict services.
            // Note: use the generic overload if you need
            // to replace the default OpenIddict entities.
            services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore()
                        .UseDbContext<EmbilyDbContext>();
                })
                .AddServer(options =>
                {
                    options.UseMvc();

                    // Enable the authorization and token endpoints (required to use the code flow).
                    options.EnableTokenEndpoint("/connect/token");

                    options.AllowPasswordFlow();
                    options.AllowRefreshTokenFlow();

                    // Accept anonymous clients (i.e clients that don't send a client_id).
                    options.AcceptAnonymousClients();

#if DEBUG
                    // During development, we can disable the HTTPS requirement that we can runs ASP.NET web server (not IISExpress) 
                    options.DisableHttpsRequirement();
#endif
                    options.RegisterScopes(
                        OpenIdConnectConstants.Scopes.OpenId,
                        OpenIdConnectConstants.Scopes.Email,
                        OpenIdConnectConstants.Scopes.Phone,
                        OpenIdConnectConstants.Scopes.Profile,
                        OpenIdConnectConstants.Scopes.OfflineAccess
                    );

                    options.SetAccessTokenLifetime(TimeSpan.FromDays(30));
                })
                .AddValidation();

            // app services
            services.AddTransient<IProgramService, ProgramService>();
            services.AddTransient<IEmailQueueSender, EmailQueueSender>();

            services.AddTransient<ICardOrder, CoinPayOrders>();
            services.AddTransient<IShippingCalc, ShippingCalc>();
            services.AddTransient<IRefGen, RefGenerator>();
            services.AddTransient<IMapper, Mapper>();


            if (env == "Development")
            {
                services.AddTransient<ICryptoAddress, CryptoAddressCreatorSim>();
            }
            else
            {
                services.AddTransient<ICryptoAddress, CryptoAddressCreator>();
            }

            //if (env == "Production")
            //{
            //    services.AddTransient<ICard, KoKardCard>();
            //    services.AddTransient<ICardLoad, KoKardCard>();
            //}
            //else
            //{
            //    services.AddTransient<ICard, VirtualCard>();
            //    services.AddTransient<ICardLoad, VirtualCard>();
            //}

            services.AddTransient<ICard, KoKardCard>();
            services.AddTransient<ICardLoad, KoKardCard>();

            // database support
            services.AddTransient<IEmbilyDbInitializer, EmbilyDbInitializer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IConfiguration configuration, IEmbilyDbInitializer embilyDbInitializer)
        {
            if (!env.IsDevelopment())
            {
                var options = new RewriteOptions().AddRedirectToHttps();
                app.UseRewriter(options);
            }

            if (!env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseAuthentication();
            app.UseSignalR(routes =>
            {
                routes.MapHub<BlockchainHub>("/hubs/blockchainhub");
            });
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api-docs/{documentName}/openapi.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api-docs/v2/openapi.json", "Embily API v2-beta1");
                c.SwaggerEndpoint("/api-docs/v1/openapi.json", "Embily API v1");
                // Display
                c.DefaultModelExpandDepth(2);
                c.DefaultModelRendering(ModelRendering.Model);
                c.DefaultModelsExpandDepth(1);
                //c.DisplayOperationId();
                c.DisplayRequestDuration();
                c.DocExpansion(DocExpansion.List);
                c.EnableDeepLinking();
                //c.EnableFilter();
                c.ShowExtensions();

                // Network
                c.EnableValidator();
                c.SupportedSubmitMethods(SubmitMethod.Get);

                // appearance 
                c.RoutePrefix = "api-docs/explorer";
                c.DocumentTitle = "Embily API Explorer";
                c.InjectStylesheet("/swagger/custom-stylesheet.css");
                c.InjectJavascript("/swagger/custom-javascript.js");

                //c.IndexStream = () => GetType().Assembly.GetManifestResourceStream("EmbilyServices.Swagger.index.html");

                c.OAuthClientId("");
                c.OAuthClientSecret("");

                //c.OAuthRealm("test-realm");
                //c.OAuthAppName("api-explorer");
                //c.OAuthScopeSeparator(" ");
                //c.OAuthAdditionalQueryStringParams(new Dictionary<string, string> {
                //     { "param1", "value1" },
                //});
                //c.OAuthUseBasicAuthenticationWithAccessCodeGrant();


            });
            app.UseReDoc(c =>
            {
                c.SpecUrl = "/api-docs/v2/openapi.json";
                c.DocumentTitle = "Embily API Documentation";
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            //app.UseSpa(spa =>
            //{
            //    // To learn more about options for serving an Angular SPA from ASP.NET Core,
            //    // see https://go.microsoft.com/fwlink/?linkid=864501

            //    spa.Options.SourcePath = "ClientApp"; // need not this one --

            //    if (env.IsDevelopment()))
            //    {


            //        //                    if (Convert.ToBoolean(configuration["RunAsProxy"]))
            //        //                    {
            //        //                        // start ng web server from PowerShell:
            //        //                        // > cd ClientApp 
            //        //                        // > ng serve 
            //        //                        spa.UseProxyToSpaDevelopmentServer("http://localhost:4200"); // Use this instead to use the angular cli server
            //        //                    }
            //        //                    else
            //        //                    {
            //        //#if DEBUG
            //        //                        //spa.UseAngularCliServer(npmScript: "start");
            //        //                        spa.UseAngularCliServer(npmScript: "build");
            //        //#else
            //        //                        spa.UseAngularCliServer(npmScript: "release");
            //        //#endif
            //        //                        spa.Options.StartupTimeout = TimeSpan.FromSeconds(60); // Increase the timeout if angular app is taking longer to startup
            //        //                    }
            //    }
            //});

            if (env.IsDevelopment() && Convert.ToBoolean(configuration["StartBuildWatch"]))
            {
                var process = new Process();
                process.StartInfo.WorkingDirectory = @".\ClientApp";
                process.StartInfo.FileName = "PowerShell";
                process.StartInfo.Arguments = @"-noExit ""del dist -recurse -force; npm run build --watch""";
                process.StartInfo.UseShellExecute = true;
                process.Start();
            }

            if (!env.IsProduction())
            {
                embilyDbInitializer.SeedAsync().Wait();
            }
        }
    }
}
