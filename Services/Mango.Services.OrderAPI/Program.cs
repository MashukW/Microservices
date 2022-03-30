using AutoMapper;
using Mango.Services.OrderAPI.Database;
using Mango.Services.OrderAPI.Database.Entities;
using Mango.Services.OrderAPI.MessageConsumers;
using Mango.Services.OrderAPI.Services;
using Mango.Services.OrderAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shared.Api.Middlewares;
using Shared.Configurations;
using Shared.Database;
using Shared.Database.Repositories;
using Shared.Message.Services.Interfaces;
using Shared.Message.Services.RabbitMq;
using Shared.Options;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<DbContextOptions<ApplicationDbContext>>(x =>
{
    var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
    optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

    return optionsBuilder.Options;
});

builder.Services.AddDbContext<BaseDbContext, ApplicationDbContext>();

builder.Services.AddHostedService<CheckoutOrderHandler>(serviceProvider =>
{
    var dbContextOptions = serviceProvider.GetService<DbContextOptions<ApplicationDbContext>>();

    using var scope = serviceProvider.CreateScope();
    var messageConsumer = scope.ServiceProvider.GetService<IMessageConsumer>();
    var messagePublisher = scope.ServiceProvider.GetService<IMessagePublisher>();
    var mapper = scope.ServiceProvider.GetService<IMapper>();
    var messageBusOptions = scope.ServiceProvider.GetService<IOptions<MessageBusOptions>>();

    return new CheckoutOrderHandler(messageBusOptions, dbContextOptions, messageConsumer, messagePublisher, mapper);
});

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<IRepository<Order>, Repository<Order>>();
builder.Services.AddScoped<IWorkUnit, WorkUnit>();

builder.Services.Configure<MessageBusOptions>(builder.Configuration.GetSection(nameof(MessageBusOptions)));

// builder.Services.AddScoped<IMessagePublisher, AzureMessagePublisher>();
// builder.Services.AddScoped<IMessageConsumer, AzureMessageConsumer>();

builder.Services.AddScoped<IMessagePublisher, RabbitMqMessagePublisher>();
builder.Services.AddScoped<IMessageConsumer, RabbitMqMessageConsumer>();

builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonOptionsConfiguration.Options.PropertyNamingPolicy;

        foreach (var converter in JsonOptionsConfiguration.Options.Converters)
            options.JsonSerializerOptions.Converters.Add(converter);
    });

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:7197/";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "mango");
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mango.Services.OrderAPI", Version = "v1" });
    c.EnableAnnotations();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter 'Bearer' [space] [token]",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using (var serviceScope = app.Services.CreateScope())
    {
        var mapper = serviceScope.ServiceProvider.GetRequiredService<IMapper>();
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseOrderPaymentSuccessUpdateStatusHandler();

app.Run();
