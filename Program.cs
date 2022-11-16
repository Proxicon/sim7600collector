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
builder.Services.AddDbContext<SimDbContext>(option =>
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


app.MapGet("/sim7600data", async (SimDbContext db) =>
    await db.Sim7600Data.Select(x => new SimDataDto(x)).ToListAsync());

app.MapGet("/sim7600data/{id}", async (int id, SimDbContext db) =>
    await db.Sim7600Data.FindAsync(id)
        is SimData Sim7600Data
            ? Results.Ok(new SimDataDto(Sim7600Data))
            : Results.NotFound());

app.MapPost("/sim7600data", async (SimDataDto Sim7600Data, SimDbContext db) =>
{
    var SimData = new SimData
    {
        Device = Sim7600Data.Device,
        Location = Sim7600Data.Location,
        Battery = Sim7600Data.Battery,
        Signal = Sim7600Data.Signal,
    };

    db.Sim7600Data.Add(SimData);
    await db.SaveChangesAsync();

    return Results.Created($"/Sim7600/{Sim7600Data.Id}", new SimDataDto(SimData));
});

app.MapGet("/sim7600logs", async (SimDbContext db) =>
    await db.Sim7600Logs.Select(x => new SimLogsDto(x)).ToListAsync());

app.MapGet("/sim7600logs/{id}", async (int id, SimDbContext db) =>
    await db.Sim7600Logs.FindAsync(id)
        is SimLogs Sim7600Logs
            ? Results.Ok(new SimLogsDto(Sim7600Logs))
            : Results.NotFound());

app.MapPost("/sim7600logs", async (SimLogsDto Sim7600LogsDto, SimDbContext db) =>
{
    var Sim7600Logs = new SimLogs
    {
        Device = Sim7600LogsDto.Device,
        Logitem = Sim7600LogsDto.Logitem,
        Message = Sim7600LogsDto.Message,
    };

    db.Sim7600Logs.Add(Sim7600Logs);
    await db.SaveChangesAsync();

    return Results.Created($"/Sim7600Logs/{Sim7600Logs.Id}", new SimLogsDto(Sim7600Logs));
});

app.Run();