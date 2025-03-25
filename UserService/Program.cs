using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using UserService.Data;
using UserService.IRepositories;
using UserService.IServices;
using UserService.Repositories;
using UserService.Services;

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

builder.Services.AddControllers();

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure SQL database context
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();

//var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!);
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
//            ValidAudience = builder.Configuration["JwtSettings:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(key)
//        };
//    });

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

// Enable Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Use CORS
app.UseCors("AllowReactApp");


// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
