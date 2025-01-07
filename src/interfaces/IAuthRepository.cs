using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catedra3Backend.src.Dtos.Auth;

namespace Catedra3Backend.src.interfaces
{
    public interface IAuthRepository
    {
        Task<AuthDto> RegisterUserAsync(RegisterDto user);
        Task<AuthDto> LogginUserAsync(LogginDto logginDto);
    }
}