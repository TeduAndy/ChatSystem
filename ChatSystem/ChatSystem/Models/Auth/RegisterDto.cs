using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace ChatSystem.Models.Auth
{
    public class RegisterDto
    {
        public string? user { get; set; }
        public string? name { get; set; }
        public string? password { get; set; }
        public string? email { get; set; }
        public string? create_date { get; set; }
        public string? up_date { get; set; }
    }
}

