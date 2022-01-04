using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Tribit.IdentityServer.App;
using Tribit.IdentityServer.Data;
using Tribit.IdentityServer.Shared.Entities;
using Tribit.IdentityServer.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<IdentityServerDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<IdentityServerDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddJWTTokenServices(builder.Configuration);

var auth = builder.Services.AddAuthentication();

AddSocialMediaAuthentication(auth, builder.Configuration);

builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

AddEmailSender(builder.Services);



builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                    }
                },
				Array.Empty<string>()
		}
    }); 
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API Documentation",
        Description = "Identity Server API"
    });
    options.DocInclusionPredicate((name, api) => true);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
   
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Server API V1");
    c.RoutePrefix = string.Empty;
});
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapDefaultControllerRoute();

app.Run();


void AddSocialMediaAuthentication(AuthenticationBuilder auth, ConfigurationManager configuration)
{
    AddGoogle(auth, configuration);
    AddFacebook(auth, configuration);
}

void AddFacebook(AuthenticationBuilder auth, ConfigurationManager configuration)
{
    IConfigurationSection facebook = configuration.GetSection("Facebook");
    if (facebook != null && facebook.GetValue<bool>("Enable"))
    {
        auth.AddFacebook(options =>
        {
            options.ClientId = facebook["ClientId"];
            options.AppSecret = facebook["AppSecret"];
        });
    }
}

void AddGoogle(AuthenticationBuilder auth, ConfigurationManager configuration)
{
    IConfigurationSection google = configuration.GetSection("Google");
    if (google != null && google.GetValue<bool>("Enable"))
    {
        auth.AddGoogle(options =>
        {
            options.ClientId = google["ClientId"];
            options.ClientSecret = google["ClientSecret"];
        });
    }
}

void AddEmailSender(IServiceCollection services)
{
    services.AddTransient<IEmailSender, NullEmailSender>();
}