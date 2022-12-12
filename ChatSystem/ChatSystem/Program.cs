using ChatSystem.Models.ConnectStr;
using ChatSystem.Models.JWT;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//// DI 注入，獲取 application token (IOption)
builder.Services.Configure<JwtDto>(builder.Configuration.GetSection("JWT"));

// DI 注入，獲取 application connectStr (組態物件)
builder.Services.AddSingleton(P =>
{
    MysqlStr connectStr = new MysqlStr();
    var applic = builder.Configuration.GetSection("connectStr");
    applic.Bind(connectStr);
    return connectStr;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

