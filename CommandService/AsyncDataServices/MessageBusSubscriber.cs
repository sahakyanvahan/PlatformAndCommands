using System.Text;
using CommandService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandService.AsyncDataServices;

public class MessageBusSubscriber : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IEventProcessor _eventProcessor;
    private IConnection _connection;
    private IModel _channel;
    private string _queuename;

    public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
    {
        _configuration = configuration;
        _eventProcessor = eventProcessor;
    }

    private void InitializeRabbitMQ()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _configuration["RabbitMQHost"], 
            Port = _configuration.GetValue<int>("RabbitMQPort")
        };
        
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
        _queuename = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(queue: _queuename, exchange: "trigger", routingKey: "");

        Console.WriteLine("--> Listening on the Message Bus...");
        
        _connection.ConnectionShutdown += OnConnectionShutdown;
    }

    public void Dispose()
    {
        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }
    }
    
    private void OnConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        Console.WriteLine("--> Connection Shutdown");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        
        InitializeRabbitMQ();
        
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (moduleHandle, ea) =>
        {
            Console.WriteLine("--> Event Received!");
            var body = ea.Body.ToArray();
            var notificationMessage = Encoding.UTF8.GetString(body);
            _eventProcessor.ProcessEvent(notificationMessage);
        };
        
        _channel.BasicConsume(queue: _queuename, autoAck: true, consumer: consumer);
        
        return Task.CompletedTask;
    }
}