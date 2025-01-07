using Bogus;
using Catedra3Backend.src.Models;
using Microsoft.AspNetCore.Identity;

namespace Catedra3Backend.src.Data
{
    public class Seeders
    {
        public static async Task Seed(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var customersContext = services.GetRequiredService<DataContext>();
                var userManager = services.GetRequiredService<UserManager<User>>();

                // Crear usuarios
                if (!customersContext.Users.Any())
                {
                    var userFaker = new Faker<User>()
                        .RuleFor(u => u.Email, f => f.Internet.Email())
                        .RuleFor(u => u.UserName, (f, u) => u.Email);
                    
                    var users = userFaker.Generate(20);

                    foreach (var user in users)
                    {
                        var result = await userManager.CreateAsync(user, "P4ssw0rd123.");

                        if (!result.Succeeded)
                        {
                            Console.WriteLine($"Error creando usuario {user.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        }
                    }
                }

                // Crear posts
                if (!customersContext.Posts.Any())
                {
                    var users = customersContext.Users.ToList();

                    foreach (var user in users)
                    {
                        var postFaker = new Faker<Post>()
                            .RuleFor(u => u.Title, f => f.Commerce.ProductName())
                            .RuleFor(u => u.publicationDate, f => f.Date.Past(18))
                            .RuleFor(u => u.UrlImage, f => f.Image.PicsumUrl())
                            .RuleFor(u => u.UserId, user.Id);
                        
                        var posts = postFaker.Generate(1);
                        customersContext.Posts.AddRange(posts);
                    }

                    customersContext.SaveChanges();
                }
            }
        }
    }
}
