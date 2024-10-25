using PuanConnect.Database;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using PuanConnect.Interfaces;
using PuanConnect.Repositories;
using AutoMapper;
using PuanConnect.Mapper;
using PuanConnect.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using PuanConnect.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;
using Coravel;
using PuanConnect.CRONJobs;
using Coravel.Scheduling.Schedule;


namespace PuanConnect
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      // Add AutoMapper
      var mapperConfig = new MapperConfiguration(mc =>
      {
        mc.AddProfile(new UserMapper());
        mc.AddProfile(new EventMapper());
        mc.AddProfile(new CategoryMapper());
        mc.AddProfile(new AttendeeMapper());
      });

      IMapper mapper = mapperConfig.CreateMapper();
      services.AddSingleton(mapper);


      // Add Services
      services.AddScoped<IUsersService, UsersService>();
      services.AddScoped<ICurrentUserService, CurrentUserService>();
      services.AddScoped<IEventsService, EventsService>();
      services.AddScoped<ICategoryService, CategoryService>();
      services.AddScoped<ICloudinaryService, CloudinaryService>();
      services.AddScoped<IEmailService, EmailService>();

      // Add Repositories
      services.AddScoped<IUsersRepository, UsersRepository>();
      services.AddScoped<IEventsRepository, EventsRepository>();
      services.AddScoped<ICategoryRepository, CategoryRepository>();

      // Add Controllers
      services.AddControllersWithViews();

      // Add Database
      services.AddDbContext<AppDBContext>(options =>
          options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

      // Add Swagger
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Documentation", Version = "v1" });
      });

      // Add Identity
      services.AddIdentity<User, IdentityRole>()
          .AddEntityFrameworkStores<AppDBContext>()
          .AddDefaultTokenProviders();
      services.AddMemoryCache();
      services.AddSession();
      services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

      // Add Authorization
      services.AddAuthorization();

      services.AddControllers().AddJsonOptions(
          x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
      );

      // Add Request User 
      services.AddHttpContextAccessor();

      services.AddTransient<GainRPInvocable>();
      services.AddTransient<CloseEventInvocable>();
      services.AddScheduler();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (!env.IsDevelopment())
      {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
      }
      else
      {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
          c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Doc Version 1");
        });
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();
      app.UseSession();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
                  name: "home",
                  pattern: "{controller=Home}/{action=Index}"
              );
        endpoints.MapControllerRoute(
                  name: "event",
                  pattern: "{controller=Event}/{action=Event}"
              );
        endpoints.MapControllerRoute(
                  name: "account",
                  pattern: "{controller=Account}/{action=Login}"
              );
        endpoints.MapControllerRoute(
                  name: "user",
                  pattern: "{controller=User}/{action=Index}"
              );
        endpoints.MapSwagger();
      });

      app.ApplicationServices.UseScheduler(scheduler =>
      {
        scheduler.OnWorker(nameof(GainRPInvocable));
        scheduler.Schedule<GainRPInvocable>()
            .DailyAt(23, 59);
      
        scheduler.OnWorker(nameof(CloseEventInvocable));
        scheduler.Schedule<CloseEventInvocable>()
            .EveryMinute();
      });

    }
  }
}