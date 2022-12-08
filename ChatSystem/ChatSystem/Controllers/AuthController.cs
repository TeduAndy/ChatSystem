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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : Controller
    {
        // connectStr 獲取
        private MysqlStr _config;

        // 建構值設定
        public AuthController(MysqlStr config)
        {
            _config = config;
        }

        // 登入
        [HttpPost]
        public async Task<ActionResult> Login()
        {
            return Ok("");
        }

        // 註冊
        [HttpPost]
        public async Task<ActionResult> Register(RegisterDto info)
        {
            try
            {
                // 查詢是否已經註冊過了
                using (var conn = new MySqlConnection(_config.MySql))
                {
                    // 判斷是否有打開連線，無則開啟
                    if (conn.State != ConnectionState.Open) conn.Open();

                    string selectStr = $@" Select user From Auth Where user = @user ";
                    var alreadRegister = await conn.QueryFirstOrDefaultAsync(selectStr, new { User = info.user });

                    // 已註冊返回錯誤
                    if (alreadRegister) return BadRequest("用戶已註冊！");

                    // 成功則返回註冊成功
                    return Ok("註冊成功");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("資料處理出現問題，註冊失敗");
            }
        }
    }
}

