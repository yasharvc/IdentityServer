using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tribit.IdentityServer.Data;
using Tribit.IdentityServer.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<IdentityServerDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<IdentityServerDbContext>();
builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

AddGoogleAuthentication(builder.Services, builder.Configuration);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();


void AddGoogleAuthentication(IServiceCollection services, ConfigurationManager configuration)
{
    var auth = services.AddAuthentication();
    IConfigurationSection google = configuration.GetSection("Google");
    if (google != null && google.GetValue<bool>("Enable")) {
        auth.AddGoogle(options =>
        {
            options.ClientId = google["ClientId"];
            options.ClientSecret = google["ClientSecret"];
        });
    }
    IConfigurationSection facebook = configuration.GetSection("Facebook");
    if (google != null && facebook.GetValue<bool>("Enable"))
    {
        auth.AddFacebook(options =>
        {
            options.ClientId = facebook["ClientId"];
            options.AppSecret = facebook["AppSecret"];
        });
    }
}