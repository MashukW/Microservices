using Duende.IdentityServer.Services;
using Mango.Services.Identity;
using Mango.Services.Identity.Database;
using Mango.Services.Identity.Database.Entities;
using Mango.Services.Identity.Initializer;
using Mango.Services.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddIdentityServer(options =>
{
	options.Events.RaiseErrorEvents = true;
	options.Events.RaiseInformationEvents = true;
	options.Events.RaiseFailureEvents = true;
	options.Events.RaiseSuccessEvents = true;

	options.EmitStaticAudienceClaim = true;
})
	.AddInMemoryIdentityResources(AppConstants.IdentityResources)
	.AddInMemoryApiScopes(AppConstants.ApiScopes)
	.AddInMemoryClients(AppConstants.Clients)
	.AddAspNetIdentity<ApplicationUser>();

builder.Services.AddScoped<DatabaseInitializer>();
builder.Services.AddScoped<IProfileService, ProfileService>();

// Add services to the container.
builder.Services.AddControllersWithViews()
	.AddJsonOptions(op =>
    {
		op.JsonSerializerOptions.DefaultBufferSize = 4096;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();

app.UseAuthorization();

var serviceProvider = app.Services;
var serviceScope = serviceProvider.CreateScope();
var dbInitializer = serviceScope.ServiceProvider.GetService<DatabaseInitializer>();

if (dbInitializer != null)
{
	await dbInitializer.Initialize();
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
