namespace Backend;

public class CampaingToolContext : Microsoft.EntityFrameworkCore.DbContext
{
    public CampaingToolContext(Microsoft.EntityFrameworkCore.DbContextOptions<CampaingToolContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CampaingToolContext).Assembly);
    }
}
