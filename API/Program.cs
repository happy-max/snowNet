
using API.Errors;
using API.Middleware;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Infrastructure.Services;
using Core.Entities.Identity;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using API.Extensions;
using Infrastructure.Data.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConnectionMultiplexer>(c =>
{
    var options = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"));
    return ConnectionMultiplexer.Connect(options);
});
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddSwaggerDocumentation();
builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));

});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = actionContext =>
    {
        var errors = actionContext.ModelState
            .Where(e => e.Value.Errors.Count > 0)
            .SelectMany(x => x.Value.Errors)
            .Select(x => x.ErrorMessage).ToArray();
        var errorResponse = new ApiValidationErrorResponse
        {
            Errors = errors
        };
        return new BadRequestObjectResult(errorResponse);
    };
});
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://locallhost:4200");
    });
});

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseCors(builder =>
{
    builder
       .WithOrigins("http://localhost:4200", "https://localhost:4200")
       .SetIsOriginAllowedToAllowWildcardSubdomains()
       .AllowAnyHeader()
       .AllowCredentials()
       .WithMethods("GET", "PUT", "POST", "DELETE", "OPTIONS")
       .SetPreflightMaxAge(TimeSpan.FromSeconds(3600));
}
);

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    var identityContext = services.GetRequiredService<AppIdentityDbContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    try
    {
        var context = services.GetRequiredService<StoreContext>();
        await context.Database.MigrateAsync();
        await StoreContextSeed.SeedAsync(context, loggerFactory);
        await identityContext.Database.MigrateAsync();
        await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occured during migration");
    }
}

app.UseStaticFiles();

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
