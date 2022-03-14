using Mango.Services.EmailAPI.Database;
using Mango.Services.EmailAPI.Extensions;
using Mango.Services.EmailAPI.Services.Interfaces;
using Mango.Services.PaymentAPI.Services;
using Microsoft.EntityFrameworkCore;
using Shared.Database;
using Shared.Message.Services;
using Shared.Message.Services.Interfaces;
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

builder.Services.AddScoped<IMessagePublisher, AzureMessagePublisher>();
builder.Services.AddScoped<IMessageConsumer, AzureMessageConsumer>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IEmailMessageConsumer, EmailMessageConsumer>();

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

app.UseSendEmailMessageConsumer();

app.Run();
