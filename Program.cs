using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RunnerMeets.Models;
using RunnerMeets.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// DI container configuration like initiate constructors/ controllers services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(options =>
{
    options.Title = "RunnerMeets API";
});
builder.Services.AddDbContext<ApplicationDbContext>(
    options =>options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .UseSnakeCaseNamingConvention());

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactDev", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();
// middleware
app.UseRewriter(new RewriteOptions().AddRedirect("task/(.*)", "todo/$1")); 

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/swagger/v1/swagger.json";
    });
}

app.UseHttpsRedirection();
app.UseCors("ReactDev");

app.UseAuthentication(); //who you are
app.UseAuthorization(); //what you can do

app.MapIdentityApi<IdentityUser>();
app.MapControllers();

//last
app.Run();
