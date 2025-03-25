using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace TransactionService.Services;

public class UserEventConsumer
{
    private readonly ConnectionFactory _factory;

    public UserEventConsumer()
    {
        _factory = new ConnectionFactory() { HostName = "localhost" };
    }

    public async Task StartListening(CancellationToken cancellationToken = default)
    {
        await using var connection = await _factory.CreateConnectionAsync(cancellationToken);
        await using var channel = await connection.CreateChannelAsync();

        // Declare main queue
        await channel.QueueDeclareAsync(
            queue: "user.created",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", "" },
                { "x-dead-letter-routing-key", "user.created.dlq" }  // DLQ setup
            },
            cancellationToken: cancellationToken
        );

        // Declare Dead Letter Queue (DLQ)
        await channel.QueueDeclareAsync(
            queue: "user.created.dlq",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken
        );

        await channel.BasicQosAsync(0, 5, false);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            try
            {
                // Deserialize message
                var userEvent = JsonSerializer.Deserialize<UserCreatedEvent>(message, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                if (userEvent == null)
                {
                    Console.WriteLine("[TransactionService] Received null event.");
                    return;
                }

                Console.WriteLine($"[TransactionService] Processing UserCreatedEvent: {userEvent.UserId}");

                // Simulate processing
                await ProcessTransactionAsync(userEvent);

                // Acknowledge successful processing
                await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TransactionService] Error processing message: {ex.Message}");

                // Reject and move message to DLQ
                await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
            }
        };

        // Start consuming messages
        await channel.BasicConsumeAsync(queue: "user.created", autoAck: false, consumer: consumer);

        Console.WriteLine("[TransactionService] Listening for events...");
        await Task.Delay(Timeout.Infinite, cancellationToken); // Keep service running
    }

    private async Task ProcessTransactionAsync(UserCreatedEvent userEvent)
    {
        // Simulate processing delay
        await Task.Delay(500);

        // Simulate random failures
        if (new Random().Next(1, 10) <= 2)  // 20% chance of failure
        {
            throw new Exception("Simulated processing failure.");
        }

        Console.WriteLine($"[TransactionService] Transaction completed for User: {userEvent.UserId}");
    }
}

// DTO for UserCreatedEvent
public class UserCreatedEvent
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }
}