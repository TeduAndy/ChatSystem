using ChatSystem.Models.ConnectStr;
using ChatSystem.Models.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Cmp;
using System.Text;

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

// 加入 JWT 驗證
builder.Services.AddAuthentication(options =>
{
    // 設定使用者預設的 Authenticate、authentication challenge 的 Scheme 都設定為 JwtBearerDefaults.AuthenticationScheme
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    // 驗證失敗時，返回失敗的原因
    options.IncludeErrorDetails = true;

    // token 驗證參數
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        // 通過這項宣告就可以從 "sub" 取值並設定給 User.Identity.Name
        NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
        // 透過這項宣告，就可以從 "roles" 取值，並可讓 [Authorize] 判斷角色
        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",

        // 驗證 Issuer
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration.GetValue<string>("JWT:Issuer"),

        // 通常不太需要驗證 Audience
        ValidateAudience = false,

        // 一般都會驗證 Token 的有效期間
        ValidateLifetime = true,

        // 如果 Token 中包含 key 才需要驗證，一般都只有簽章而已
        ValidateIssuerSigningKey = false,

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JWT:key")))
    };

    // 驗證失敗時自訂錯誤
    options.Events = new JwtBearerEvents()
    {
        OnChallenge = context =>
        {
            // 此處終止默認 jwt 驗證失敗的返回類型及數據結果 (很重要)
            context.HandleResponse();

            // 自定義 需要返回的數據類型及結果, 通過引用 Newtonsoft.Json 進行轉換
            var payload = JsonConvert.SerializeObject(new { Code="401", ErrorMessage="無權限訪問！" });

            // 設置返回數據類型
            context.Response.ContentType = "application/json";

            // 返回默認狀態碼
            context.Response.StatusCode = 401;

            // 輸出數據結果
            context.Response.WriteAsync(payload);

            return Task.FromResult(0);
        }
    };

});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// https 重定向
//app.UseHttpsRedirection();


//      jwt
//     先驗證
app.UseAuthentication();
//     再授權
app.UseAuthorization();

app.MapControllers();

app.Run();

