using AutoMapper;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using ComicBookShopCore.Data.Repositories;
using ComicBookShopCore.Services;
using ComicBookShopCore.Services.Artist;
using ComicBookShopCore.Services.Publisher;
using ComicBookShopCore.Services.Series;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            services.AddControllers();
            var mappingConfig = new MapperConfiguration(mc =>
                mc.AddProfile(new MapperProfile()));
            IMapper mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);
            services.AddSingleton<DbContext>(new ShopDbEntities(Configuration.GetSection("CONNECTION_STRING").Value));
            services.AddSingleton<IAsyncArtistRepository, EfAsyncArtistRepository>();
	    services.AddSingleton<IAsyncPublisherRepository, EfAsyncPublisherRepository>();
            services.AddSingleton<IAsyncSeriesRepository, EfAsyncSeriesRepository>();
            services.AddSingleton(new ShopDbEntities(Configuration.GetSection("CONNECTION_STRING").Value));
            services.AddSingleton<IArtistService,ArtistService>();
            services.AddSingleton<IPublisherService, PublisherService>();
            services.AddScoped<ISeriesService, SeriesService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

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
