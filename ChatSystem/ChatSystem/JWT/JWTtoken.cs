using ChatSystem.Models.JWT;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace ChatSystem.JWT
{
    public class JWTtoken
    {
        public static async Task<string> createToken(JwtDto _jwt, string user, int agingMinutes = 30)
        {
            // 獲取組態設定 JWT key 跟 發行人
            string issuer = _jwt.Issuer;
            string key = _jwt.key;

            // 加入 payload 相關資訊
            List<Claim> claims = new List<Claim>();

            // 加入使用者帳號
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user)); // 引用官方，驗證通過後可以直接使用 User.Identity.Name 獲取用戶帳號
            // 加入 Token 時效
            claims.Add(new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddMinutes(agingMinutes).ToUnixTimeSeconds().ToString()));
            // JWTID
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

          
            ClaimsIdentity userClaimsIdentity = new ClaimsIdentity(claims);

            // 為 token 產生 key
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.key));

            // 將 SymmetricSecurityKey 轉成 HAS256
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // 創建 token 描述符
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = issuer,
                Subject = userClaimsIdentity,
                SigningCredentials = signingCredentials,
            };

            // 生成 JWT securityToken，並轉成字串
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            string token = tokenHandler.WriteToken(securityToken);

            // 返回 token
            return token;
        }
    }
}
