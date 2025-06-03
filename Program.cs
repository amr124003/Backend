using myapp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using myapp.middlewares;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using myapp.Data;

var builder = WebApplication.CreateBuilder(args);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services for controllers

// Register ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services for controllers
builder.Services.AddControllers();
builder.Services.AddScoped<myapp.auth.Services.PaymentService>();
builder.Services.AddScoped<myapp.auth.Services.EmailService>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add configuration
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// Register TokenService
builder.Services.AddScoped<ITokenService, TokenService>();

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
