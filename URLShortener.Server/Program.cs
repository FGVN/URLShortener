using URLShortener.Server.Auth;
using URLShortener.Server.Data;
using URLShortener.Server.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient<UrlMetadataService>();
builder.Services.AddSingleton<EncryptionService>();

builder.Services.AddSingleton(new JwtTokenService(
    builder.Configuration["Jwt:Issuer"],
    builder.Configuration["Jwt:Audience"],
    builder.Configuration["Jwt:SecretKey"]
));

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
