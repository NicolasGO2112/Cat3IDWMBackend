using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Catedra3Backend.src.Models
{
    public class User : IdentityUser<int>
    {
    }
}