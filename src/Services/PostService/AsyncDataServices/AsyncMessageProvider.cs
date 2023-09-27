using System.Text;
using System.Text.Json;
using PostService.Dtos;
using RabbitMQ.Client;

namespace PostService.AsyncDataServices;

public class AsyncMessageProvider : IAsyncMessageProvider
{
    private readonly IConfiguration _config;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public AsyncMessageProvider(IConfiguration config)
    {
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
            Console.WriteLine($"--> Connected to message bus");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Can't connect to the message bus: {ex.Message}");
        }
    }

    public void PublishPostCreated(PostCreatePublishDto postCreatePublish)
    {
        postCreatePublish.Event = "post_created";
        var message = JsonSerializer.Serialize(postCreatePublish);

        if (_connection.IsOpen)
        {
            Console.WriteLine($"--> Sending message");
            SendMessage(message);
        }
        else
        {
            Console.WriteLine($"--> Can't send message. Connection closed.");
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
        Console.WriteLine($"--> Message send: {message}");
    }

    private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        Console.WriteLine($"--> Message bus connection shutdown");
    }

    public void Dispose()
    {
        Console.WriteLine("--> Message bus disposed");
        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }
    }
}