using Mango.Services.EmailAPI.Database;
using Mango.Services.EmailAPI.Services.Interfaces;
using Mango.Services.OrderAPI.MessageConsumers;
using Mango.Services.PaymentAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Database;
using Shared.Message.Services.Interfaces;
using Shared.Message.Services.RabbitMq;
using Shared.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<DbContextOptions<ApplicationDbContext>>(x =>
{
    var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
    optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

    return optionsBuilder.Options;
});

builder.Services.AddDbContext<BaseDbContext, ApplicationDbContext>();

builder.Services.Configure<MessageBusOptions>(builder.Configuration.GetSection(nameof(MessageBusOptions)));

builder.Services.AddScoped<IEmailService, EmailService>();

// builder.Services.AddScoped<IMessageConsumer, AzureMessageConsumer>();
builder.Services.AddScoped<IMessageConsumer, RabbitMqMessageConsumer>();

builder.Services.AddHostedService<SendEmailHandler>(serviceProvider =>
{
    var dbContextOptions = serviceProvider.GetService<DbContextOptions<ApplicationDbContext>>();

    using var scope = serviceProvider.CreateScope();
    var messageBusOptions = scope.ServiceProvider.GetService<IOptions<MessageBusOptions>>();
    var messageConsumer = scope.ServiceProvider.GetService<IMessageConsumer>();
    var emailService = scope.ServiceProvider.GetService<IEmailService>();

    return new SendEmailHandler(messageBusOptions, dbContextOptions, messageConsumer, emailService);
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
