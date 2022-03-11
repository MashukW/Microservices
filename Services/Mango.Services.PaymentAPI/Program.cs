using Mango.Services.OrderAPI.Services.Interfaces;
using Mango.Services.PaymentAPI.Extensions;
using Mango.Services.PaymentAPI.Services;
using Mango.Services.PaymentAPI.Services.Interfaces;
using Shared.Api.Middlewares;
using Shared.Message.Services;
using Shared.Message.Services.Interfaces;
using Shared.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MessageBusOptions>(builder.Configuration.GetSection(nameof(MessageBusOptions)));

builder.Services.AddScoped<IMessagePublisher, AzureMessagePublisher>();
builder.Services.AddScoped<IMessageConsumer, AzureMessageConsumer>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentMessageConsumer, PaymentMessageConsumer>();

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

app.UsePaymentMessageConsumer();

app.Run();
