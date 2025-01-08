using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Catedra3Backend.src.Dtos.Post
{
    public class UploadPostDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "El titulo debe tener al menos 5 caracteres.")]
        public string Title {get;set;} =string.Empty; 
        [Required]
        public DateTime publicationDate {get;set;}  
        [Required]
        public IFormFile Image {get;set;} 
        public int UserId {get;set;} 

    }
}