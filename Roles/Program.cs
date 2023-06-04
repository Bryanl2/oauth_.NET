using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication("cookie")
        .AddCookie("cookie");
builder.Services.AddDataProtection();
builder.Services.AddAuthorization();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.Use((ctx, next) =>
//{
//    //var idp = ctx.RequestServices.GetRequiredService<IDataProtectionProvider>();
//    //var protector = idp.CreateProtector("auth-cookie");
//    //var authcookie = ctx.Request.Headers.Cookie.FirstOrDefault(x => x.StartsWith("auth="));
//    //if (authcookie != null)
//    //{
//    //    var protectorPayload = authcookie.Split("=").Last();
//    //    var payload = protector.Unprotect(protectorPayload);
//    //    var parts = payload.Split(":");
//    //    var key = parts[0];
//    //    var value = parts[1];
//    //}


//    return next();
//});
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
