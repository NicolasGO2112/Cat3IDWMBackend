using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catedra3Backend.src.Dtos.Post;
using Catedra3Backend.src.Models;

namespace Catedra3Backend.src.interfaces
{
    public interface IPostRepository
    {
        Task<Post> CreateNewPost(UploadPostDto uploadPostDto);
        Task<IEnumerable<ViewPostDto>> GetAllPost();
        
    }
}