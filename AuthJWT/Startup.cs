using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthJWT.DataAcces;
using AuthJWT.Options;
using AuthJWT.Services;
using AuthJWT.Services.PublicPins;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace AuthJWT
{
    public class Startup
    {
        readonly string AllowSpecificOrigins = "_myAllowSpecificOrigins";

        private readonly IConfiguration configuration;
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            /* services.Configure<SecretOptions>(configuration.GetSection("Secrets"));*/
            services.AddDbContext<UserContext>(options => options.UseSqlServer(configuration.GetConnectionString("MapConnectionString")));
            services.AddDbContext<ModeratePinContext>(options => options.UseSqlServer(configuration.GetConnectionString("MapConnectionString")));
            services.AddDbContext<PublicPinContext>(options => options.UseSqlServer(configuration.GetConnectionString("MapConnectionString")));
            services.AddDbContext<SolvedPinContext>(options => options.UseSqlServer(configuration.GetConnectionString("MapConnectionString")));

            services.AddTransient<AuthService>();
            services.AddTransient<PublicPinServiceCRUD>();
            services.AddTransient<ModerationPinService>();
            services.AddTransient<SolvedPinContext>();

            services.AddSingleton<PublicPinServiceGet>();

            services.AddCors(o => o.AddPolicy("FrontPolicy", builder =>
            {
                builder.WithOrigins("http://localhost:3000")
                                   .AllowAnyHeader()
                                   .WithMethods("GET")
                                   .WithMethods("POST")
                                   .AllowCredentials();
            }));

            services.AddCors(o => o.AddPolicy("ModerationPolicy", builder =>
            {
                builder.WithOrigins("http://localhost:4343")
                                   .AllowAnyHeader()
                                   .AllowAnyMethod()
                                   .AllowCredentials();
            }));

            services.AddControllers();

            /*var secrets = configuration.GetSection("Secrets");
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
*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors("FrontPolicy");
            app.UseCors("ModerationPolicy");

            /*  app.UseAuthentication(); */
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
