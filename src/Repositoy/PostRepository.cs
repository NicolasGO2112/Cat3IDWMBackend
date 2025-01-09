using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catedra3Backend.src.Data;
using Catedra3Backend.src.Dtos.Post;
using Catedra3Backend.src.interfaces;
using Catedra3Backend.src.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;

namespace Catedra3Backend.src.Repositoy
{
    public class PostRepository : IPostRepository
    {
        private readonly DataContext _dataContext;
        private readonly Cloudinary _cloudinary;
        
        public PostRepository(DataContext dataContext, Cloudinary cloudinary)
        {
            _dataContext = dataContext;
            _cloudinary = cloudinary;
        }
        public async Task<Post> CreateNewPost(UploadPostDto uploadPostDto)
        {
            if (uploadPostDto.Image == null)
            {
                return null;
            }
            if (uploadPostDto.Image.ContentType != "image/png" && uploadPostDto.Image.ContentType != "image/jpg"  && uploadPostDto.Image.ContentType != "image/jpeg" )
            {
                return null;
            }
            if(uploadPostDto.Image.Length > 5 * 1024 * 1024){
                return null;
            }
            // Realiza los párametros de subida de la imagen
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(uploadPostDto.Image.FileName, uploadPostDto.Image.OpenReadStream()),
                Folder = "post_image"
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult.Error != null)
            {
                throw new Exception(uploadResult.Error.Message);
            }
            var post = new Post
            {
                Title = uploadPostDto.Title,
                publicationDate = uploadPostDto.publicationDate,
                UrlImage = uploadResult.SecureUrl.AbsoluteUri,
                UserId = uploadPostDto.UserId
                
            };
            // Agrega el producto a la base de datos
            _dataContext.Posts.Add(post);
            await _dataContext.SaveChangesAsync();
            return post;
        }

        public async Task<IEnumerable<ViewPostDto>> GetAllPost()
        {
            var posts = await _dataContext.Posts.ToListAsync();
            var viewPostsDto = new List<ViewPostDto>();
            foreach (var post in posts)
            {
                viewPostsDto.Add(
                    new ViewPostDto{   
                        Title = post.Title,
                        publicationDate = post.publicationDate,
                        UrlImage = post.UrlImage
                    });
            }
            return viewPostsDto;
        }
    }
}