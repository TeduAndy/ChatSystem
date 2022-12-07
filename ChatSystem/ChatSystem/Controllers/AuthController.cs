using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatSystem.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        // 登入
        [HttpPost]
        public void Login([FromBody]string value)
        {
        }

        // 註冊
        [HttpPost]
        public void Register([FromBody] string value)
        {
        }
    }
}

