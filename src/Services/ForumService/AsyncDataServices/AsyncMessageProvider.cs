using System.Text;
using System.Text.Json;
using ForumService.Dtos;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace ForumService.AsyncDataServices;

public class AsyncMessageProvider : IAsyncMessageProvider
{
    private readonly ILogger _logger;
    private readonly IConfiguration _config;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public AsyncMessageProvider(ILogger<AsyncMessageProvider> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
        var factory = new ConnectionFactory()
        {
            HostName = _config["RabbitMQ:Host"],
            Port = Convert.ToInt32(_config["RabbitMQ:Port"]) 
        };
        
        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            _logger.LogInformation($"--> Connected to message bus");
        }
        catch (BrokerUnreachableException)
        {
            _logger.LogError("--> Message bus not reachable");
        }
        catch (Exception ex)
        {
            _logger.LogError("--> Error while connection to message bus: {message}", ex.Message);
        }
    }

    public void PublishPostCreated(PostCreatePublishDto postCreatePublish)
    {
        if (_connection.IsOpen)
        {
            postCreatePublish.Event = "post_created";
            var message = JsonSerializer.Serialize(postCreatePublish);

            _logger.LogInformation($"--> Sending message");
            SendMessage(message);
        }
        else
        {
            _logger.LogInformation($"--> Can't send message. Connection closed.");
        }
    }

    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(
            exchange: "trigger",
            routingKey: "",
            basicProperties: null,
            body: body);
        _logger.LogInformation("--> Message send: {message}", message);
    }

    private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        _logger.LogInformation($"--> Message bus connection shutdown");
    }

    public void Dispose()
    {
        _logger.LogInformation("--> Message bus disposed");
        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }
    }
}