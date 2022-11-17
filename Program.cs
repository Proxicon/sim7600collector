
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using sim7600collector.Data;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseKestrel(options => options.AddServerHeader = false);

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = new HeaderApiVersionReader("api-version");
});

builder.Services.AddHttpContextAccessor();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new OpenApiInfo()
    {
        Description = "Web api implementation using Minimal Api in Asp.Net Core",
        Title = "Sim Device Data Collector",
        Version = "v1",
        Contact = new OpenApiContact()
        {
            Name = "Bostec",
            Url = new Uri("https://bostec.co.za/")
        }
    });

    setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    setup.OperationFilter<AddAuthorizationHeaderOperationFilter>();
    setup.OperationFilter<AddVersionHeaderOperationFilter>();
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.MaxDepth = 64;
    options.JsonSerializerOptions.IncludeFields = true;
});

// Auth
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateActor = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Issuer"],
            ValidAudience = builder.Configuration["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SigningKey"]))
        };
    });

// Data

// builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Use in memory db
//builder.Services.AddDbContext<Sim7600Db>(opt => opt.UseInMemoryDatabase("Sim7600DataList"));

// Use SqlLite DB
/*var connectionString = builder.Configuration.GetConnectionString("Sim7600Data") ?? "Data Source=sim7600collector.db";
builder.Services.AddSqlite<Sim7600Db>(connectionString);*/

// Use SqlServer
builder.Services.AddDbContext<SimDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetValue<string>("SqlServer:ConnectionString")));

// Use SqlServer context factory
/*builder.Services.AddDbContextFactory<SimDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetValue<string>("SqlServer:ConnectionString")));*/

builder.Services.AddHealthChecks().AddDbContextCheck<SimDbContext>();

builder.Services.AddScoped<IValidator<UserInput>, UserInputValidator>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});


// API
app.MapPost("/token", async (SimDbContext db, HttpContext http, UserInput userInput, IValidator<UserInput> userInputValidator) =>
{
    var validationResult = userInputValidator.Validate(userInput);
    if (!validationResult.IsValid)
    {
        return Results.BadRequest();
    }

    var loggedInUser = await db._users
        .FirstOrDefaultAsync(user => user.Username == userInput.Username
        && user.Password == userInput.Password);

    if (loggedInUser == null)
    {
        return Results.Unauthorized();
    }

    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, loggedInUser.Username!),
        new Claim(JwtRegisteredClaimNames.Name, loggedInUser.Username!),
        new Claim(JwtRegisteredClaimNames.Email, loggedInUser.Email!)
    };

    var token = new JwtSecurityToken
    (
        issuer: builder.Configuration["Issuer"],
        audience: builder.Configuration["Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddDays(60),
        notBefore: DateTime.UtcNow,
        signingCredentials: new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SigningKey"])),
            SecurityAlgorithms.HmacSha256)
    );

    return Results.Json(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
}).WithTags("Authentication").Accepts<UserInput>("application/json").Produces(200).Produces(401).ProducesProblem(StatusCodes.Status400BadRequest);

app.MapGet("/health", async (HealthCheckService healthCheckService) =>
{
    var report = await healthCheckService.CheckHealthAsync();
    return report.Status == HealthStatus.Healthy ? Results.Ok(report) : Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
}).WithTags(new[] { "Health" }).Produces(200).ProducesProblem(503).ProducesProblem(401);

app.MapGet("/simdata", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async (SimDbContext db) =>
{
    var data = await db._simData.Select(x => new SimDataDto(x)).ToListAsync();
    return data;

}).WithTags(new[] {"StateData"}).Produces(200).ProducesProblem(503).ProducesProblem(401);

app.MapGet("/simdata/{id}", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async (int id, SimDbContext db) =>
    await db._simData.FindAsync(id)
        is SimData Sim7600Data
            ? Results.Ok(new SimDataDto(Sim7600Data))
            : Results.NotFound());

app.MapPost("/simdata", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async (SimDataDto Sim7600Data, SimDbContext db) =>
{
    var SimData = new SimData
    {
        Device = Sim7600Data.Device,
        Location = Sim7600Data.Location,
        Battery = Sim7600Data.Battery,
        Signal = Sim7600Data.Signal,
    };

    db._simData.Add(SimData);
    await db.SaveChangesAsync();

    return Results.Created($"/Sim7600/{Sim7600Data.Id}", new SimDataDto(SimData));
});

app.MapGet("/simlogs", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async (SimDbContext db) =>
    await db._simLogs.Select(x => new SimLogsDto(x)).ToListAsync());

app.MapGet("/simlogs/{id}", async (int id, SimDbContext db) =>
    await db._simLogs.FindAsync(id)
        is SimLogs Sim7600Logs
            ? Results.Ok(new SimLogsDto(Sim7600Logs))
            : Results.NotFound());

app.MapPost("/simlogs", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async (SimLogsDto Sim7600LogsDto, SimDbContext db) =>
{
    var Sim7600Logs = new SimLogs
    {
        Device = Sim7600LogsDto.Device,
        Logitem = Sim7600LogsDto.Logitem,
        Message = Sim7600LogsDto.Message,
    };

    db._simLogs.Add(Sim7600Logs);
    await db.SaveChangesAsync();

    return Results.Created($"/Sim7600Logs/{Sim7600Logs.Id}", new SimLogsDto(Sim7600Logs));
});

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.Run();