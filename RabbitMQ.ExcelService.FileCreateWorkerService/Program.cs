using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.ExcelService.FileCreateWorkerService;
using RabbitMQ.ExcelService.FileCreateWorkerService.Models;
using RabbitMQ.ExcelService.FileCreateWorkerService.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        services.AddHostedService<Worker>();
        services.AddSingleton(sp => new ConnectionFactory()
        { Uri = new Uri(configuration.GetConnectionString("RabbitMQ")), DispatchConsumersAsync = true });

        services.AddDbContext<RabbitMqexcelDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
        });

        services.AddSingleton<RabbitMQClientService>();

    })
    .Build();

host.Run();
