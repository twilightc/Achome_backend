using AchomeModels.DbModels;
using AchomeModels.Models;
using AchomeModels.Models.RequestModels;
using AchomeModels.Models.ResponseModels;
using AchomeModels.Service;
using AchomeModels.Service.Implement;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Text;

namespace wokyoubuliauchieguan
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var settingsSection = Configuration.GetSection("ApplicationSettings");
            var settings = settingsSection.Get<ApplicationSettings>();
            services.Configure<ApplicationSettings>(settingsSection);

            AddJWT(services, settings);
            AddSwagger(services);
            AddMapper(services);
            AddServices(services, settings);
            services.AddControllersWithViews().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null; //new PascalCase()
            });
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }
        private static void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    name: "v1",// name: 攸關 SwaggerDocument 的 URL 位置。
                    info: new OpenApiInfo// info: 是用於 SwaggerDocument 版本資訊的顯示(內容非必填)。
                    {
                        Title = "RESTful API",
                        Version = "1.0.0",
                        Description = "This is ASP.NET Core RESTful API Sample."
                    }
                );
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "AchomeWeb.xml");
                c.IncludeXmlComments(xmlPath);
            });
        }
        private static void AddJWT(IServiceCollection services, ApplicationSettings settings)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.JWT_Secret));
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }
        private static void AddMapper(IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Merchandise, MerchandiseViewModel>();
                cfg.CreateMap<MerchandiseSpec, MerchandiseSpecViewModel>();
                cfg.CreateMap<MerchandiseQa, MerchandiseQaViewModel>();
                cfg.CreateMap<TransportMethod, TransportMethodViewModel>();
                cfg.CreateMap<SevenElevenShop, SevenElevenShopViewModel>();
                cfg.CreateMap<AddMerchandiseRequestModel, Merchandise>();
                cfg.CreateMap<AddSpecModel, MerchandiseSpec>();
            });
            IMapper iMapper = config.CreateMapper();
            services.AddSingleton(iMapper);
        }
        private static void AddServices(IServiceCollection services, ApplicationSettings settings)
        {
            services.AddDbContext<AChomeContext>(options => options.UseSqlServer(settings.IdentityConnection));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMerchandiseService, MerchandiseService>();
            services.AddScoped<ICheckoutService, CheckoutService>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<IEstablishedOrderService, EstablishedOrderService>();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(
                    url: "/swagger/v1/swagger.json",// url: 需配合 SwaggerDoc 的 name。 "/swagger/{SwaggerDoc name}/swagger.json"
                    name: "RESTful API v1.0.0"// description: 用於 Swagger UI 右上角選擇不同版本的 SwaggerDocument 顯示名稱使用。
                );
            });
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }
            app.UseCors(builder => builder
                                  //.AllowAnyOrigin()
                                  .WithOrigins("http://nexifytw.mynetgear.com:9527", "http://localhost:4200", "http://localhost:3000")
                                  .AllowAnyHeader()
                                  .AllowAnyMethod()
                                  .SetIsOriginAllowed((host) => true)
                                  //.AllowCredentials()
                                  );
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();//先驗證、再授權

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
