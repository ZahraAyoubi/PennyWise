using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using System.Threading;
using RabbitMQ.Client.Exceptions;

namespace TransactionService.Services;

public class TransactionEventPublisher
{
    private readonly ConnectionFactory _factory;

    public TransactionEventPublisher()
    {
        _factory = new ConnectionFactory() { HostName = "localhost" };
    }

    public async Task PublishTransactionCreatedEvent(Guid transactionId, Guid userId, decimal amount)
    {
        await using var connection = await _factory.CreateConnectionAsync(); // Async connection
        await using var channel = await connection.CreateChannelAsync();     // Async channel (IChannel)

        //channel.QueueDeclare(queue: "transaction.created", durable: true, exclusive: false, autoDelete: false, arguments: null);
        // Declare Queue (Async)
        await channel.QueueDeclareAsync(
            queue: "transaction.created",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );


        var transactionEvent = new TransactionCreatedEvent { TransactionId = transactionId, UserId = userId, Amount = amount, CreatedAt = DateTime.UtcNow };
        //var messageBody = Encoding.UTF8.GetBytes(
        //    JsonSerializer.Serialize(transactionEvent, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
        //);
        var messageBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(transactionEvent));


        try
        {
            var basicProperties = new BasicProperties();

            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: "transaction.created",
                mandatory: false,
                basicProperties: basicProperties,
                body: messageBody
            );

            Console.WriteLine($"[TransactionService] Sent TransactionCreatedEvent: {transactionId}");
        }


        catch (AlreadyClosedException ex)
        {
            Console.WriteLine($"RabbitMQ connection closed: {ex.Message}");
        }

       
        

        //channel.BasicPublish(exchange: "", routingKey: "transaction.created", basicProperties: null, body: messageBody);

        Console.WriteLine($"[TransactionService] Sent TransactionCreatedEvent: {transactionId}");
    }
}
public class TransactionCreatedEvent
{
    public Guid TransactionId { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}