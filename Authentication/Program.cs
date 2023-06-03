using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddDataProtection();
//builder.Services.AddHttpContextAccessor();
//builder.Services.AddScoped<AuthService>();
builder.Services.AddAuthentication("cookie")
    .AddCookie("cookie");
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.UseAuthentication();

//app.Use((ctx, next) =>
//{
//    var idp = ctx.RequestServices.GetRequiredService<IDataProtectionProvider>();
//    var protector = idp.CreateProtector("auth-cookie");
//    var authcookie = ctx.Request.Headers.Cookie.FirstOrDefault(x => x.StartsWith("auth="));
//    if(authcookie != null)
//    {
//        var protectorPayload = authcookie.Split("=").Last();
//        var payload = protector.Unprotect(protectorPayload);
//        var parts = payload.Split(":");
//        var key = parts[0];
//        var value = parts[1];

//        var claims = new List<Claim>();
//        claims.Add(new Claim(key, value));
//        var identity = new ClaimsIdentity(claims);
//        ctx.User = new ClaimsPrincipal(identity);

//    }
//    else
//        Console.WriteLine("Request doesn't contains cookies");
//    return next();
//});

app.MapGet("/username", (HttpContext ctx, IDataProtectionProvider idp) =>
{

    return ctx.User.FindFirst("usr").Value;
});

app.MapGet("/login", async (HttpContext ctx) =>
{
    var claims = new List<Claim>();
    claims.Add(new Claim("usr", "Bryan"));
    var identity = new ClaimsIdentity(claims, "cookie");
    var user = new ClaimsPrincipal(identity);
    await ctx.SignInAsync("cookie",user);
    return "ok";
});

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

//public class AuthService 
//{
//    private readonly IDataProtectionProvider _idp;
//    private readonly IHttpContextAccessor _accesor;

//    public AuthService(IDataProtectionProvider idp, IHttpContextAccessor accesor)
//    {
//        _idp = idp;
//        _accesor = accesor;
//    }

//    public void SignIn()
//    {
//        var protector = _idp.CreateProtector("auth-cookie");
//        _accesor.HttpContext.Response.Headers["set-cookie"] = $"auth={protector.Protect("usr:bryan")}";
//    }
//}