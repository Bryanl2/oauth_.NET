using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
const string AuthScheme = "cookie";
const string AuthSchemeTwo = "cookie2";
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(AuthScheme)
    .AddCookie(AuthScheme);


builder.Services.AddAuthorization(builder =>
{
    builder.AddPolicy("la_passport", pl =>
    {
        pl.RequireAuthenticatedUser()
          .AddAuthenticationSchemes(AuthScheme)
          .AddRequirements(new Requirement())
          .RequireClaim("passport_type", "la");
    });
    builder.AddPolicy("eu_passport", pl =>
    {
        pl.RequireAuthenticatedUser()
          .AddAuthenticationSchemes(AuthScheme)
          .RequireClaim("passport_type", "eu");
    });
    builder.AddPolicy("ti_passport", pl =>
    {
        pl.RequireAuthenticatedUser()
          .AddAuthenticationSchemes(AuthScheme)
          .RequireClaim("passport_type", "ti");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

//app.Use((ctx, next) =>
//{

//    if (ctx.Request.Path.StartsWithSegments("/login"))
//    {
//        return next();
//    }

//    if (!ctx.User.Identities.Any(x => x.AuthenticationType == AuthScheme))
//    {
//        ctx.Response.StatusCode = 401;
//        return Task.CompletedTask;
//    }

//    if (!ctx.User.HasClaim("passport_type", "ti"))
//    {
//        ctx.Response.StatusCode = 403;
//        return Task.CompletedTask;
//    }
//    return next();
//});

app.MapGet("/unsecure", (HttpContext ctx) =>
{
   
    return ctx.User.FindFirst("usr")?.Value ?? "empty";
});

app.MapGet("/colombia", (HttpContext ctx) =>
{

    //var lstClaims = ctx.User.Claims.ToList();

    //if(!ctx.User.Identities.Any(x => x.AuthenticationType == AuthScheme))
    //{
    //    ctx.Response.StatusCode = 401;
    //    return "";
    //}

    //if (!ctx.User.HasClaim("passport_type", "la"))
    //{
    //    ctx.Response.StatusCode = 403;
    //    return "";
    //}
    
    return "allowed";
}).RequireAuthorization("la_passport");
//[Authorize(Policy="la_passport")] --> equivalent in MVC

app.MapGet("/tibet", (HttpContext ctx) =>
{
    //var lstClaims = ctx.User.Claims.ToList();

    //if (!ctx.User.Identities.Any(x => x.AuthenticationType == AuthSchemeTwo))
    //{
    //    ctx.Response.StatusCode = 401;
    //    return "";
    //}

    //if (!ctx.User.HasClaim("passport_type", "ti"))
    //{
    //    ctx.Response.StatusCode = 403;
    //    return "";
    //}
    return "allowed";
}).RequireAuthorization("ti_passport");

//[AuthScheme(AuthScheme)]
//[AuthClaim("passport_type","au")] In normal controllers
app.MapGet("/australia", (HttpContext ctx) =>
{

    //var lstClaims = ctx.User.Claims.ToList();

    //if (!ctx.User.Identities.Any(x => x.AuthenticationType == AuthSchemeTwo))
    //{
    //    ctx.Response.StatusCode = 401;
    //    return "";
    //}

    //if (!ctx.User.HasClaim("passport_type", "au"))
    //{
    //    ctx.Response.StatusCode = 403;
    //    return "";
    //}

    return "allowed";
}).RequireAuthorization("eu_passport");

//[AllowAnonymous] in real controller
app.MapGet("/login", async (HttpContext cxt) =>
{
    //Here we can implement logic for switching the auth scheme based on roles
    var claim = new List<Claim>();

    //Here is needed role's logic per user, for example getting roles based on users returning a determined claim's type and value
    claim.Add(new Claim("usr", "Bryan"));
    claim.Add(new Claim("passport_type", "la"));
    var identity = new ClaimsIdentity(claim,AuthScheme);
    var user = new ClaimsPrincipal(identity);
    await cxt.SignInAsync(AuthScheme,user);
}).AllowAnonymous();

app.Run();


public class Requirement:IAuthorizationRequirement
{

}

public class RequirementHandler : AuthorizationHandler<Requirement>
{
    //DI for database's connection
    public RequirementHandler()
    {

    }
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, Requirement requirement)
    {
        //context.User
        //context.Suceed(new Requirement());
        return Task.CompletedTask;
    }
}
