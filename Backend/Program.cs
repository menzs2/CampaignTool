using Backend;
using Backend.Data;
using Backend.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:7099")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configure EF Core with PostgreSQL
builder.Services.AddDbContext<CampaignToolContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<CreateDefaultCampaign>();

// Register the Services
builder.Services.AddScoped<CampaignService>();
builder.Services.AddScoped<CharacterService>();
builder.Services.AddScoped<ConnectionService>();
builder.Services.AddScoped<OrganisationService>();
builder.Services.AddScoped<UserService>();

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
    else if (!dbContext.Database.GetPendingMigrations().Any())
    {
        // If the database is created but no migrations are pending, we can still ensure it's up to date
        dbContext.Database.Migrate();
    }
    else if (dbContext.Database.GetPendingMigrations().Any())
    {
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
app.UseAuthorization();
app.MapControllers();
app.UseRouting();
app.UseCors();
app.Run();