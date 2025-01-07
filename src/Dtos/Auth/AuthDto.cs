using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Catedra3Backend.src.Dtos.Auth
{
    public class AuthDto : IdentityUser<int>
    {

        public string Token { get; set; } = string.Empty;
        
    }
}