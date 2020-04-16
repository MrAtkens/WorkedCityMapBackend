using AuthJWT.DataAcces;
using AuthJWT.Services;
using AuthJWT.Services.PublicPins;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.IO;

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
            services.AddDirectoryBrowser();
            /* services.Configure<SecretOptions>(configuration.GetSection("Secrets"));*/
            services.AddDbContext<UserContext>(options => options.UseSqlServer(configuration.GetConnectionString("MapConnectionString")));
            services.AddDbContext<ModerateContext>(options => options.UseSqlServer(configuration.GetConnectionString("ModerateConnectionString")));
            services.AddDbContext<PinsContext>(options => options.UseSqlServer(configuration.GetConnectionString("MapConnectionString")));

            services.AddTransient<AuthService>();
            services.AddTransient<PublicPinServiceCRUD>();
            services.AddTransient<ModerationPinService>();
            services.AddTransient<SolvedPinService>();

            services.AddTransient<PublicPinServiceGet>();

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

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Images")),
                RequestPath = "/Images"
            });


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
