using Microsoft.EntityFrameworkCore;

namespace sim7600collector.Data;

public class SimDbContext : DbContext
{
        public SimDbContext(DbContextOptions<SimDbContext> options)
            : base(options) { }

        public DbSet<SimData> Sim7600Data => Set<SimData>();
        public DbSet<SimLogs> Sim7600Logs => Set<SimLogs>();
}
