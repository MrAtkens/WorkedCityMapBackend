using AuthJWT.DataAcces;
using AuthJWT.Options;
using AuthJWT.Services;
using AuthJWT.Services.PublicPins;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthJWT
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDirectoryBrowser();
            services.Configure<SecretOptions>(configuration.GetSection("Secrets"));
            services.AddDbContext<UserContext>(options => options.UseSqlServer(configuration.GetConnectionString("MapConnectionString")));
            services.AddDbContext<ModerateContext>(options => options.UseSqlServer(configuration.GetConnectionString("ModerateConnectionString")));
            services.AddDbContext<PinsContext>(options => options.UseSqlServer(configuration.GetConnectionString("MapConnectionString")));

            services.AddTransient<PublicPinServiceCRUD>();
            services.AddTransient<ModerationPinService>();
            services.AddTransient<SolvedPinService>();

            services.AddTransient<PublicPinServiceGet>();

            services.AddScoped<UserAuthService>();

            services.AddMemoryCache();

            services.AddCors(o => 
            {
                o.AddPolicy(CorsOrigins.FrontPolicy, builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                                       .AllowAnyHeader()
                                       .WithMethods("GET")
                                       .WithMethods("POST")
                                       .AllowCredentials();
                });

                o.AddPolicy(CorsOrigins.AdminPanelPolicy, builder =>
                {
                    builder.WithOrigins("http://localhost:4343")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            services.AddDirectoryBrowser();

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            var secrets = configuration.GetSection("Secrets");
            var key = Encoding.ASCII.GetBytes(secrets.GetValue<string>("JWTSecret"));
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseFileServer(enableDirectoryBrowsing: false);
            /*  app.UseDirectoryBrowser(new DirectoryBrowserOptions
              {
                  FileProvider = new PhysicalFileProvider(
                  Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
                  RequestPath = "/Uploads"
              });*/


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            
            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
