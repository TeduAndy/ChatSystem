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
                using (var tr = new TransactionScope())
                using (var conn = new MySqlConnection(_config.MySql))
                {
                    // 判斷是否有打開連線，無則開啟
                    if (conn.State != ConnectionState.Open) conn.Open();

                    string user = info.user;
                    string name = info.name;
                    string password = info.password;
                    string email = info.email;
                    string create_date = DateTime.Now.ToString("yyyy-MM-DD"); // 時間抓取當前時間
                    string up_date = DateTime.Now.ToString("yyyy-MM-DD"); // 時間抓取當前時間


                    // 查詢SQL
                    string selectStr = $@" Select user From Auth Where user = @user ";

                    // 執行查詢
                    var alreadRegister = await conn.QueryFirstOrDefaultAsync(selectStr, new { User = info.user });

                    // 已註冊返回錯誤
                    if (!string.IsNullOrEmpty(alreadRegister)) return BadRequest("用戶已註冊！");

                    // 注入SQL
                    try
                    {
                        //string insertStr = $@" Insert Into Auth(user, name, password, email, create_date, up_date) 
                        //                   Values(@user, @name, @password, @email, @create_date, @up_date)  ";
                        string sql = $@" Insert Into Auth(user, name, password, email, create_date, up_date)
                                           Values('2', @name, @password, @email, @create_date, @up_date)  ";
                        var result = await conn.ExecuteAsync(sql, new
                        {
                            User = 1,
                            Name = info.name,
                            Password = info.password,
                            Email = info.email,
                            Create_date = info.create_date,
                            Up_date = info.up_date
                        });

                        //string insertStr = $@" Insert Into Auth(user, name, password, email, create_date, up_date) 
                        //                   Values(1, @name, @password, @email, @create_date, @up_date)  ";
                        string insertStr = "asdafaf";
                        // 執行注入
                        result = await conn.ExecuteAsync(insertStr, new
                        {
                            User = 1,
                            Name = info.name,
                            Password = info.password,
                            Email = info.email,
                            Create_date = info.create_date,
                            Up_date = info.up_date
                        });
                        tr.Complete();
                    }
                    catch (Exception)
                    {
                        //tr.Rollback(); // 失敗則回滾資料
                        return BadRequest("創建失敗");
                    }


                    // 成功注入資料庫則則返回註冊成功 
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

