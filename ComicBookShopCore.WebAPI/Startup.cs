using System.IdentityModel.Tokens.Jwt;
using System.Text;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using ComicBookShopCore.Data;
using ComicBookShopCore.Services;
using ComicBookShopCore.Services.Artist;
using ComicBookShopCore.Services.ComicBook;
using ComicBookShopCore.Services.Publisher;
using ComicBookShopCore.Services.Series;
using ComicBookShopCore.Services.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace ComicBookShopCore.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().AddNewtonsoftJson();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperProfile());
                mc.AddCollectionMappers();
            });

            IMapper mapper = mappingConfig.CreateMapper();

            services.AddCors();

            services.AddSingleton(mapper);
            services.AddSingleton<DbContext>(new ShopDbEntities(Configuration.GetSection("CONNECTION_STRING").Value));
            services.AddSingleton<IAsyncArtistRepository, EfAsyncArtistRepository>();
            services.AddSingleton<IAsyncPublisherRepository, EfAsyncPublisherRepository>();
            services.AddSingleton<IAsyncSeriesRepository, EfAsyncSeriesRepository>();
            services.AddSingleton<IComicBookRepositoryAsync, EfComicBookRepositoryAsync>();
            services.AddSingleton(new ShopDbEntities(Configuration.GetSection("CONNECTION_STRING").Value));
            services.AddSingleton<IArtistService, ArtistService>();
            services.AddSingleton<IPublisherService, PublisherService>();
            services.AddScoped<ISeriesService, SeriesService>();
            services.AddSingleton<IComicBookService, ComicBookService>();
            services.AddScoped<IUserService, UserService>();
            services.AddIdentityCore<User>().AddRoles<IdentityRole>().AddEntityFrameworkStores<ShopDbEntities>()
                .AddDefaultTokenProviders();

            var key = Encoding.ASCII.GetBytes("TX0BudRrGZ37Ymwi7mnf");
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("Admin", policy => policy.RequireClaim("Role", "Admin"));
                opt.AddPolicy("Employee", policy => policy.RequireClaim("Role", "Admin", "Employee"));
            });

            services.AddMvcCore();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();

        }
    }
}
