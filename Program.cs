using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.MaxDepth = 64;
    options.JsonSerializerOptions.IncludeFields = true;
});

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Use in memory db
//builder.Services.AddDbContext<Sim7600Db>(opt => opt.UseInMemoryDatabase("Sim7600DataList"));

// Use SqlLite DB
/*var connectionString = builder.Configuration.GetConnectionString("Sim7600Data") ?? "Data Source=sim7600collector.db";
builder.Services.AddSqlite<Sim7600Db>(connectionString);*/

// Use SqlServer
builder.Services.AddDbContext<Sim7600Db>(option =>
    option.UseSqlServer(builder.Configuration.GetValue<string>("SqlServer:ConnectionString")));

// create initial migration
// dotnet ef migrations add InitialCreate
// dotnet ef database update

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});


app.MapGet("/sim7600data", async (Sim7600Db db) =>
    await db.Sim7600Data.Select(x => new Sim7600DataDTO(x)).ToListAsync());

app.MapGet("/sim7600data/{id}", async (int id, Sim7600Db db) =>
    await db.Sim7600Data.FindAsync(id)
        is Sim7600Data Sim7600Data
            ? Results.Ok(new Sim7600DataDTO(Sim7600Data))
            : Results.NotFound());

app.MapPost("/sim7600data", async (Sim7600DataDTO Sim7600DataDTO, Sim7600Db db) =>
{
    var Sim7600Data = new Sim7600Data
    {
        Device = Sim7600DataDTO.Device,
        Location = Sim7600DataDTO.Location,
        Battery = Sim7600DataDTO.Battery,
        Signal = Sim7600DataDTO.Signal,
    };

    db.Sim7600Data.Add(Sim7600Data);
    await db.SaveChangesAsync();

    return Results.Created($"/Sim7600/{Sim7600Data.Id}", new Sim7600DataDTO(Sim7600Data));
});

app.MapGet("/sim7600logs", async (Sim7600Db db) =>
    await db.Sim7600Logs.Select(x => new Sim7600LogsDTO(x)).ToListAsync());

app.MapGet("/sim7600logs/{id}", async (int id, Sim7600Db db) =>
    await db.Sim7600Logs.FindAsync(id)
        is Sim7600Logs Sim7600Logs
            ? Results.Ok(new Sim7600LogsDTO(Sim7600Logs))
            : Results.NotFound());

app.MapPost("/sim7600logs", async (Sim7600LogsDTO Sim7600LogsDTO, Sim7600Db db) =>
{
    var Sim7600Logs = new Sim7600Logs
    {
        Device = Sim7600LogsDTO.Device,
        Logitem = Sim7600LogsDTO.Logitem,
        Message = Sim7600LogsDTO.Message,
    };

    db.Sim7600Logs.Add(Sim7600Logs);
    await db.SaveChangesAsync();

    return Results.Created($"/Sim7600Logs/{Sim7600Logs.Id}", new Sim7600LogsDTO(Sim7600Logs));
});

app.Run();

public class Sim7600Data
{
    public int Id { get; set; }
    public string? Device { get; set; }    
    public string? Location { get; set; }
    public string? Battery { get; set; }
    public int Signal { get; set; }
}

public class Sim7600DataDTO
{
    public int Id { get; set; }
    public string? Device { get; set; }
    public string? Location { get; set; }
    public string? Battery { get; set; }
    public int Signal { get; set; }

    public Sim7600DataDTO() { }
    public Sim7600DataDTO(Sim7600Data Sim7600DataItem) =>
    (Id, Device, Location, Battery, Signal) = (Sim7600DataItem.Id, 
                                               Sim7600DataItem.Device, 
                                               Sim7600DataItem.Location, 
                                               Sim7600DataItem.Battery,
                                               Sim7600DataItem.Signal);
}

public class Sim7600Logs
{
    public int Id { get; set; }
    public string? Device { get; set; }
    public string? Logitem { get; set; }
    public string? Message { get; set; }
}

public class Sim7600LogsDTO
{
    public int Id { get; set; }
    public string? Device { get; set; }
    public string? Logitem { get; set; }
    public string? Message { get; set; }

    public Sim7600LogsDTO() { }
    public Sim7600LogsDTO(Sim7600Logs Sim7600Logs) =>
    (Id, Device, Logitem, Message) = (Sim7600Logs.Id,
                                      Sim7600Logs.Device,
                                      Sim7600Logs.Logitem,
                                      Sim7600Logs.Message);
}

class Sim7600Db : DbContext
{
    public Sim7600Db(DbContextOptions<Sim7600Db> options)
        : base(options) { }

    public DbSet<Sim7600Data> Sim7600Data => Set<Sim7600Data>();
    public DbSet<Sim7600Logs> Sim7600Logs => Set<Sim7600Logs>();
}
