using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication()
    .AddScheme<CookieAuthenticationOptions, VisitorAuthHandler>("visitor",x => { } )
    .AddCookie("local")
    .AddCookie("patreon-cookie")
    .AddOAuth("external-authentication",x =>
    {
        x.SignInScheme = "patreon-cookie";
        x.ClientId = "test";
        x.ClientSecret = "test";
        x.AuthorizationEndpoint = "https://oauth.mocklab.io/oauth/authorize";
        x.TokenEndpoint = "https://oauth.mocklab.io/oauth/token";
        x.UserInformationEndpoint = "https://oauth.mocklab.io/userinfo";
        x.CallbackPath = "/patreon";
        x.Scope.Add("profile");
        x.SaveTokens = true;
    });

builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("customer", pl =>
    {
        pl.AddAuthenticationSchemes("patreon-cookie", "local", "visitor")
          .RequireAuthenticatedUser();
    });

    x.AddPolicy("user", pl =>
    {
        pl.AddAuthenticationSchemes("local")
          .RequireAuthenticatedUser();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", ctx => Task.FromResult("Hello world")).RequireAuthorization("customer");

app.MapGet("/login-local", async (ctx) =>
{
    var claims = new List<Claim>();
    claims.Add(new Claim("usr", "bryan"));
    var identity = new ClaimsIdentity(claims, "local");
    var user = new ClaimsPrincipal(identity);

    await ctx.SignInAsync("local", user);
}).RequireAuthorization("customer");

app.MapGet("/login-patreon", async (ctx) =>
{
    await ctx.ChallengeAsync("external-authentication", new AuthenticationProperties()
    {
        RedirectUri = "/"
    });
}).RequireAuthorization("user");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UsePathBase("/");
app.Run();


public class VisitorAuthHandler : CookieAuthenticationHandler
{
    public VisitorAuthHandler(IOptionsMonitor<CookieAuthenticationOptions> options, ILoggerFactory logger,
      UrlEncoder encoder,
      ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var result = await base.HandleAuthenticateAsync();

        if (result.Succeeded)
        {
            return result;
        }

        var claims = new List<Claim>();
        claims.Add(new Claim("usr", "bryan"));
        var identity = new ClaimsIdentity(claims, "visitor");
        var user = new ClaimsPrincipal(identity);
        await Context.SignInAsync("visitor",user);
        return AuthenticateResult.Success(new AuthenticationTicket(user,"visitor"));
    }
}