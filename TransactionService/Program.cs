using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using TransactionService.Data;
using TransactionService.IServices;
using TransactionService.IRepositories;
using TransactionService.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks()
    .AddRabbitMQ(sp =>
    {
        var factory = new ConnectionFactory()
        {
            Uri = new Uri("amqp://guest:guest@localhost:5672") // Set RabbitMQ URI
        };
        return factory.CreateConnectionAsync(); // Return IConnection
    }, name: "rabbitmq");

builder.Services.AddControllers(); // For API controllers, if applicable

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure SQL database context
builder.Services.AddDbContext<TransactionDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITransactionService, TransactionService.Services.TransactionService>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

// Enable CORS for React App
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy.WithOrigins("http://localhost:49222")  // React's URL
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

var app = builder.Build();

app.MapHealthChecks("/health");

// Enable Swagger in development or all environments
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Optional: Configure UI path if needed
}

// Use CORS
app.UseCors("AllowReactApp");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
