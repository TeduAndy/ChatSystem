using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatSystem.Models.Auth;
using ChatSystem.Models.ConnectStr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Dapper;
using MySql.Data.MySqlClient;
using System.Transactions;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using ChatSystem.Models.JWT;
using ChatSystem.JWT;

namespace ChatSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : Controller
    {
        // connectStr 獲取
        private readonly MysqlStr _config;
        private readonly JwtDto _jwt;

        // 建構值設定
        public AuthController(MysqlStr config, IOptions<JwtDto> jwt)
        {
            _config = config;
            _jwt = jwt.Value;
        }

        // 登入
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login(LoginDto info)
        {
            string user = info.user;
            string password = Convert.ToBase64String(Encoding.UTF8.GetBytes(info.password)); // 將明文轉成 base64 字串

            string sql = @" Select user, password From Auth Where user = @user And password = @password ";

            try
            {
                using (var conn = new MySqlConnection(_config.MySql))
                {
                    // 打開連線
                    if(conn.State != ConnectionState.Open) conn.Open();

                    // 找出是否有此用戶
                    LoginDto result = await conn.QueryFirstOrDefaultAsync<LoginDto>(sql, new { user, password });

                    // 無的話返回找不到用戶
                    if (result == null) return NotFound("查無此用戶");

                    // 有的話則返回 Token(JWT)
                    string token = await JWTtoken.createToken(_jwt, result.user);

                    return Ok(token);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("出現異常錯誤");
            }
        }

        // 註冊
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Register(RegisterDto info)
        {
            try
            {
                string user = info.user;
                string name = info.name;
                string password = Convert.ToBase64String(Encoding.UTF8.GetBytes(info.password)); // 將密碼轉成 base 編碼
                string email = info.email;
                string create_date = DateTime.Now.ToString("yyyy-MM-dd"); // 時間抓取當前時間
                string up_date = DateTime.Now.ToString("yyyy-MM-dd"); // 時間抓取當前時間

                // 查詢是否已經註冊過了
                using (var tr = new TransactionScope())
                using (var conn = new MySqlConnection(_config.MySql))
                {
                    // 判斷是否有打開連線，無則開啟
                    if (conn.State != ConnectionState.Open) conn.Open();

                    // 查詢SQL
                    string selectStr = $@" Select user From Auth Where user = @user ";

                    // 執行查詢
                    var alreadRegister = await conn.QueryFirstOrDefaultAsync(selectStr, new { User = info.user });

                    // 已註冊返回錯誤
                    if (!string.IsNullOrEmpty(alreadRegister)) return BadRequest("用戶已註冊！");

                    // 注入SQL
                    string insertStr = $@" Insert Into Auth(user, name, password, email, create_date, up_date) 
                                        Values(@user, @name, @password, @email, @create_date, @up_date)  ";
                       
                    // 執行注入
                    int result = await conn.ExecuteAsync(insertStr, new { user, name, password, email, create_date, up_date });
                    
                    // 執行交易成功或者交易失敗
                    tr.Complete();
                   
                    // 成功注入資料庫則則返回註冊成功 
                    return Ok("註冊成功");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("資料處理出現問題，註冊失敗");
            }
        }

        // 修改
        [Authorize]
        [HttpPut]
        public async Task<ActionResult> UpdateInfo(UpdateDto info)
        {
            return Ok("OK");
        }
    }
}

