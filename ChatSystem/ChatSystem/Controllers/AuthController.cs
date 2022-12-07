using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatSystem.Models.Auth;
using ChatSystem.Models.ConnectStr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatSystem.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AuthController : Controller
    {
        // connectStr 獲取
        private IOptions<MysqlStr> _config;
        private MysqlStr _config2;

        // 建構值設定
        public AuthController(IOptions<MysqlStr> config, MysqlStr config2)
        {
            _config = config;
            _config2 = config2;
        }

        // 登入
        [HttpGet]
        public async Task<ActionResult> Login()
        {
            return Ok(_config.Value.MySql);
        }

        // 註冊
        [HttpGet]
        public async Task<ActionResult> Register()
        {

            return Ok(_config2);
        }
    }
}

