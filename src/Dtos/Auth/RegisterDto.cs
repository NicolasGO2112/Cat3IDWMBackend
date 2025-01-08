using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Catedra3Backend.src.Dtos.Auth
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public string Mail {get;set;} = string.Empty;
        [Required]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        [RegularExpression(@"^(?=.*\d).{6,}$", ErrorMessage = "La contraseña debe tener al menos 6 caracteres y contener al menos un número.")]
        public string Password {get;set;} = string.Empty;
    }
}