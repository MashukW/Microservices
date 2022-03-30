using Mango.Services.OrderAPI.MessageConsumers;
using Mango.Services.PaymentAPI.Services;
using Mango.Services.PaymentAPI.Services.Interfaces;
using Microsoft.Extensions.Options;
using Shared.Api.Middlewares;
using Shared.Message.Services.Interfaces;
using Shared.Message.Services.RabbitMq;
using Shared.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MessageBusOptions>(builder.Configuration.GetSection(nameof(MessageBusOptions)));

builder.Services.AddScoped<IPaymentService, PaymentService>();

// builder.Services.AddScoped<IMessagePublisher, AzureMessagePublisher>();
// builder.Services.AddScoped<IMessageConsumer, AzureMessageConsumer>();

builder.Services.AddScoped<IMessagePublisher, RabbitMqMessagePublisher>();
builder.Services.AddScoped<IMessageConsumer, RabbitMqMessageConsumer>();

builder.Services.AddHostedService<OrderPaymentRequestHandler>(serviceProvider =>
{
    using var scope = serviceProvider.CreateScope();
    var messageConsumer = scope.ServiceProvider.GetService<IMessageConsumer>();
    var messagePublisher = scope.ServiceProvider.GetService<IMessagePublisher>();
    var paymentService = scope.ServiceProvider.GetService<IPaymentService>();
    var messageBusOptions = scope.ServiceProvider.GetService<IOptions<MessageBusOptions>>();

    return new OrderPaymentRequestHandler(messageBusOptions, paymentService, messageConsumer, messagePublisher);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
