using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catedra3Backend.src.Models;

namespace Catedra3Backend.src.interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(User user);
        
    }
}