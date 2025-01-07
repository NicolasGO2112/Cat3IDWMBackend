using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catedra3Backend.src.Dtos.Post;

namespace Catedra3Backend.src.interfaces
{
    public interface IPostRepository
    {
        Task<string> CreateNewPost(UploadPostDto uploadPostDto);
        Task<ViewPostDto> GetAllPost();
        
    }
}