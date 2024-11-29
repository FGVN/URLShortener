using URLShortener.Server.Auth;
using URLShortener.Server.Data;
using URLShortener.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using URLShortener.Server.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient<UrlMetadataService>();
builder.Services.AddSingleton<EncryptionService>();

builder.Services.AddSingleton(new JwtTokenService(
    builder.Configuration["Jwt:Issuer"],
    builder.Configuration["Jwt:Audience"],
    builder.Configuration["Jwt:SecretKey"]
));

var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admins", policy => policy.RequireRole("Admins"));
    options.AddPolicy("AuthorizedUsers", policy => policy.RequireRole("AuthorizedUsers"));
});

// Add CORS configuration to allow all origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Allow all origins
              .AllowAnyMethod()  // Allow all HTTP methods (GET, POST, PUT, DELETE, etc.)
              .AllowAnyHeader(); // Allow all headers
    });
});

var app = builder.Build();

// Apply CORS policy globally
app.UseCors("AllowAll");

app.UseDefaultFiles();
app.MapControllerRoute(
    name: "shorturlRedirect",
    pattern: "{shortUrl}",
    defaults: new { controller = "UrlShortener", action = "RedirectToOriginal" });

app.UseStaticFiles();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<RedirectMiddleware>();

app.MapFallbackToFile("/index.html");

app.Run();
