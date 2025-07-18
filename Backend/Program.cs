using Backend.Data;
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

// Configure EF Core with PostgreSQL
builder.Services.AddDbContext<CampaignToolContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<CreateDefaultCampaign>();

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
app.Run();