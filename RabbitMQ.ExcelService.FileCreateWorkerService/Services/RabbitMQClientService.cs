using RabbitMQ.Client;

namespace RabbitMQ.ExcelService.FileCreateWorkerService.Services;

public class RabbitMQClientService : IDisposable
{
    private readonly ConnectionFactory _connectionFactory;
    private IConnection _connection;
    private IModel _channel;

    public static string RoutingExcel = "excel-route-file";
    public static string QueueName = "queue-excel-file";

    private readonly ILogger<RabbitMQClientService> _logger;

    public RabbitMQClientService(ConnectionFactory connectionFactory, ILogger<RabbitMQClientService> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public IModel Connect()
    {
        _connection = _connectionFactory.CreateConnection();

        if (_channel is { IsOpen: true })
        {
            return _channel;
        }

        _channel = _connection.CreateModel();

        _logger.LogInformation("RabbitMQ İle bağlantı kuruldu...");

        return _channel;
    }

    public void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();

        _connection?.Close();
        _connection?.Dispose();

        _logger.LogInformation("RabbitMQ İle bağlantı koptu...");
    }
}