using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catedra3Backend.src.Data;
using Catedra3Backend.src.Dtos.Auth;
using Catedra3Backend.src.interfaces;
using Catedra3Backend.src.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Catedra3Backend.src.Repositoy
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signInManager;

        public AuthRepository(DataContext dataContext, UserManager<User> userManager, ITokenService tokenService, SignInManager<User> signInManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }
        public async Task<AuthDto> LogginUserAsync(LogginDto logginDto)
        {
            string normalizedEmail = logginDto.Mail.ToUpper();
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.NormalizedEmail == normalizedEmail);
            if(user == null) return null;

            var result = await _signInManager.CheckPasswordSignInAsync(user, logginDto.Password, false);
            if (!result.Succeeded) return null;

            var newUser = await _tokenService.CreateToken(user);
            var auth = new AuthDto
            {
                Id = user.Id,
                Email = user.Email!,
                Token = newUser
            };
            return auth;
        }
        public async Task<AuthDto> RegisterUserAsync(RegisterDto user)
        {      
            User newUser = new User{
                Email = user.Mail,
                UserName = user.Mail // Puedes usar el correo como nombre de usuario
            };
            // Obtiene el resultado de la creación del usuario
            Console.WriteLine("XXDXDXDDD");

            var result = await _userManager.CreateAsync(newUser, user.Password);
            // Si la creación fue exitosa
            if (result.Succeeded)
            {
                await _dataContext.SaveChangesAsync();
            }
            // Si la creación falló
            else
            {
                throw new Exception("Error creating user");
            }
            // Crea un token para el usuario
            var registeredUser = await _tokenService.CreateToken(newUser);
            var auth = new AuthDto
            {
                Id = newUser.Id,
                Email = newUser.Email!,
                Token = registeredUser
            };
            return auth;
        }
    
        public async Task<bool> ExistEmail(string email)
        {
            bool exist = await _userManager.FindByEmailAsync(email) != null;
            return exist;

        }
    }
}