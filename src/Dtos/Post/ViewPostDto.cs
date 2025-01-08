using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catedra3Backend.src.Dtos.Post
{
    public class ViewPostDto
    {
        public string Title {get;set;} =string.Empty; 
        public DateTime publicationDate {get;set;}  
        public string UrlImage {get;set;} = string.Empty;
    }
}