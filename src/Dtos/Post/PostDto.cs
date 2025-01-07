using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Catedra3Backend.src.Dtos.Post
{
    public class PostDto
    {
        [Key]
        [Required]
        public int PostId {get;set;} 
        [Required]

        public string Title {get;set;} =string.Empty; 
        [Required]

        public DateTime publicationDate {get;set;}  
        [Required]

        public string UrlImage {get;set;} =string.Empty; 
        [Required]
        public int UserId {get;set;} 
    }
}