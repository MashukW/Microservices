using Mango.Services.ShoppingCartAPI.Database;
using Mango.Services.ShoppingCartAPI.Database.Entities;
using Mango.Services.ShoppingCartAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shared.Configurations;
using Shared.Database;
using Shared.Database.Repositories;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly()); // AppDomain.CurrentDomain.GetAssemblies() Assembly.GetAssembly(typeof(MyApp.SomeClass)

builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddScoped<BaseDbContext, ApplicationDbContext>();

builder.Services.AddScoped<IRepository<Cart>, Repository<Cart>>();
builder.Services.AddScoped<IRepository<CartItem>, Repository<CartItem>>();
builder.Services.AddScoped<IRepository<CartProduct>, Repository<CartProduct>>();

builder.Services.AddScoped<IWorkUnit, WorkUnit>();

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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mango.Services.ShoppingCartApi", Version = "v1" });
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
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
