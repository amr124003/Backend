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
        var signingKey = builder.Configuration["Jwt:SigningKey"];
        if (string.IsNullOrEmpty(signingKey))
        {
            throw new InvalidOperationException("JWT SigningKey is not configured.");
        }

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(signingKey)),
            
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty; // Set Swagger UI at the root
    });
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

// Add your custom AuthMiddleware after UseAuthentication and UseAuthorization
app.UseMiddleware<AuthMiddleware>();

app.MapControllers();

app.Run();
