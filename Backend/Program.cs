using Backend;
using Backend.Data;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Campaign Tool API",
        Version = "v1",
        Description = "API documentation for the Campaign Tool project"
    });

    // Enable XML comments for Swagger
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Add Cors policy
// Read allowed CORS origins from configuration
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(allowedOrigins ?? Array.Empty<string>())
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

//Configure Authentication
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
.AddEntityFrameworkStores<CampaignToolContext>();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]))
        };
    });

builder.Services.AddAuthorization();

// Configure EF Core with PostgreSQL
builder.Services.AddDbContext<CampaignToolContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<CreateDefaultCampaign>();

// Register the Services
builder.Services.AddAuthentication();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<CampaignService>();
builder.Services.AddScoped<CharacterService>();
builder.Services.AddScoped<ConnectionService>();
builder.Services.AddScoped<OrganisationService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped(provider => jwtSettings);
var app = builder.Build();

// Apply migrations at startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CampaignToolContext>();
    if (dbContext.Database.EnsureCreated())
    {
        // If the database is created, we can seed it with default data
        // Ensure the database is created and truncate tables if they exist 
        app.Services.CreateScope().ServiceProvider.GetRequiredService<CreateDefaultCampaign>().Execute();
    }
    else if (dbContext.Database.GetPendingMigrations().Any())
    {
        // Apply any pending migrations
        dbContext.Database.Migrate();
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();
app.Run();