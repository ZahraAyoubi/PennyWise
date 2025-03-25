using System.Text;
using System.Text.Json;
using Microsoft.Identity.Client;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace UserService.Services;

public class UserEventPublisher
{
    private readonly ConnectionFactory _factory;

    public UserEventPublisher()
    {
        _factory = new ConnectionFactory() { HostName = "localhost" };
    }

    public async Task PublishUserCreatedEvent(Guid userId, string userName, CancellationToken cancellationToken = default)
    {
        await using var connection = await _factory.CreateConnectionAsync(cancellationToken); // Async connection
        await using var channel = await connection.CreateChannelAsync();     // Async channel (IChannel)

        // Declare Queue (Async)
        await channel.QueueDeclareAsync(
            queue: "user.created",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken
        );

        // Create message (Fixed JSON serialization)
        var userEvent = new { UserId = userId, UserName = userName };
        var messageBody = Encoding.UTF8.GetBytes(
            JsonSerializer.Serialize(userEvent, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
        );

        try
        {
            var basicProperties = new BasicProperties();
            
            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: "user.created",
                mandatory: false,
                basicProperties: basicProperties, 
                body: messageBody,
                cancellationToken: cancellationToken
            );

            Console.WriteLine($"[UserService] Sent UserCreatedEvent: {userId}");
        }
        catch (AlreadyClosedException ex)
        {
            Console.WriteLine($"RabbitMQ connection closed: {ex.Message}");
        }
    }
}
