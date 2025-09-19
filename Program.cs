using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using myapp.Data;
using myapp.Mapping;
using myapp.middlewares;
using myapp.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });
});

// Add services for controllers

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);



builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
// Add services for controllers
builder.Services.AddControllers();
builder.Services.AddScoped<myapp.auth.Services.PaymentService>();
builder.Services.AddScoped<myapp.auth.Services.EmailService>();

// Register ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("Server =.;Initial Catalog=Nexus;Integrated Security=SSPI;Trust Server Certificate=True;"));

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add configuration

// Register TokenService
builder.Services.AddScoped<ITokenService, TokenService>();

#region Mapster

var config = TypeAdapterConfig.GlobalSettings;
config.Scan(typeof(MappingConfig).Assembly);

builder.Services.AddSingleton(config);
builder.Services.AddScoped<IMapper, ServiceMapper>();

#endregion

// Register UserDataAccess
builder.Services.AddScoped<myapp.DataAccess.UserDataAccess>();

// Add authorization services
builder.Services.AddAuthorization();

// Add authentication services
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Configure JWT bearer options
        var signingKey = builder.Configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(signingKey))
        {
            throw new InvalidOperationException("JWT SigningKey is not configured.");
        }

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddSignalR();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty; // Set Swagger UI at the root
});

app.UseAuthentication();
app.UseAuthorization();

// Add your custom AuthMiddleware after UseAuthentication and UseAuthorization
app.UseMiddleware<AuthMiddleware>();

app.MapControllers();


// Add this in the endpoint configuration section
app.MapHub<myapp.auth.Hubs.LeaderboardHub>("/leaderboardHub");

app.Run();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        dbContext.Database.CanConnect();
        Console.WriteLine("Database connection successful.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database connection failed: {ex.Message}");
    }
}
