using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace sim7600collector.Data;

public class SimDbContext : DbContext
{
        public SimDbContext(DbContextOptions<SimDbContext> options)
            : base(options) { }

    public DbSet<SimData> _simData => Set<SimData>();
    public DbSet<SimLogs> _simLogs => Set<SimLogs>();
    public DbSet<User> _users => Set<User>();
}
