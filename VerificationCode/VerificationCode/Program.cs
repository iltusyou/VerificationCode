using Microsoft.EntityFrameworkCore;
using VerificationCode.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<WebContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("WebDatabase")));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();