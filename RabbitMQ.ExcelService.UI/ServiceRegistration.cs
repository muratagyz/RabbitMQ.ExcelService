using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.ExcelService.UI.Models;
using RabbitMQ.ExcelService.UI.Services;

namespace RabbitMQ.ExcelService.UI;

public static class ServiceRegistration
{
    public static IServiceCollection AddCustomServiceCollection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSignalR();

        services.AddSingleton(sp => new ConnectionFactory()
            { Uri = new Uri(configuration.GetConnectionString("RabbitMQ")), DispatchConsumersAsync = true });

        services.AddSingleton<RabbitMQClientService>();

        services.AddSingleton<RabbitMQPublisher>();

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
        });

        services.AddIdentity<IdentityUser, IdentityRole>(opt =>
        {
            opt.User.RequireUniqueEmail = true;

        }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

        using (var scope = services.BuildServiceProvider().CreateScope())
        {
            var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            appDbContext.Database.Migrate();

            if (!appDbContext.Users.Any())
            {
                userManager.CreateAsync(new IdentityUser()
                {
                    UserName = "test",
                    Email = "test@gmail.com",
                }, "Test.123").Wait();

                userManager.CreateAsync(new IdentityUser()
                {
                    UserName = "test2",
                    Email = "test2@gmail.com",
                }, "Test.123").Wait();
            }
        }

        return services;
    }
}