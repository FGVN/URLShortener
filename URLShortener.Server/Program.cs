using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using URLShortener.Server.Handlers;
using URLShortener.Services;
using URLShortener.Infrastructure.Data;
using URLShortener.Infrastructure.MiddleWares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient<UrlMetadataService>();
builder.Services.AddSingleton<EncryptionService>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<RegisterHandler>());

builder.Services.AddSingleton(new JwtTokenService(
    builder.Configuration["Jwt:Issuer"]!,
    builder.Configuration["Jwt:Audience"]!,
    builder.Configuration["Jwt:SecretKey"]!
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"] ?? string.Empty))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admins", policy => policy.RequireRole("Admins"));
    options.AddPolicy("AuthorizedUsers", policy => policy.RequireRole("AuthorizedUsers"));
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  
              .AllowAnyMethod()  
              .AllowAnyHeader(); 
    });
});

var app = builder.Build();

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
