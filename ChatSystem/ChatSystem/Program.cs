using ChatSystem.Models.ConnectStr;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//// DI 注入，獲取 application connectStr 方法一
//builder.Services.Configure<MysqlStr>(builder.Configuration.GetSection("connectStr"));

// DI 注入，獲取 application connectStr 方法二
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

