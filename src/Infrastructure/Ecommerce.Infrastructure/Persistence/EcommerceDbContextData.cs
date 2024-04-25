using Ecommerce.Application.Models.Authorization;
using Ecommerce.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Persistence
{
    public class EcommerceDbContextData
    {
        public static async Task LoadDataAsync(
            EcommerceDbContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            ILoggerFactory loggerFactory
            )
        {
            try
            {
                if(!roleManager.Roles.Any())
                {
                    await roleManager.CreateAsync(new IdentityRole(Role.ADMIN));
                    await roleManager.CreateAsync(new IdentityRole(Role.USER));
                }

                if (!userManager.Users!.Any())
                {
                    var userAdmin = new User
                    {
                        Name= "Arlex",
                        LastName="Guzman",
                        Email="arlexrush@gmail.com",
                        UserName= "arlexrush",
                        PhoneNumber="+34672245944",
                        AvatarUrl= "https://pixabay.com/es/vectors/hombre-persona-avatar-rostro-156584/"
                    };

                    if (!userManager.Users!.Contains(userAdmin))
                    {
                        var addData1=await userManager.CreateAsync(userAdmin, "Audir8lemas#");
                        await userManager.AddToRoleAsync(userAdmin, Role.ADMIN);
                    }
                    
                    var userData = new User
                    {
                        Name = "Jose",
                        LastName = "Perez",
                        Email = "jose.perez@gmail.com",
                        UserName = "joseperez",
                        PhoneNumber = "+584126003279",
                        AvatarUrl = "https://pixabay.com/es/vectors/empresario-masculino-negocio-avatar-310819/"
                    };

                    var ifContain = userManager.Users!.Contains(userData);
                    if (!userManager.Users!.Contains(userData))                    
                    {
                        var addData2=await userManager.CreateAsync(userData, "Playgirl01#");
                        await userManager.AddToRoleAsync(userData, Role.USER);
                    }
                    
                }

                if(!context.Categories!.Any())
                {
                    var categoriesData = File.ReadAllText(@"C:\Users\arlex\source\REPOS_.NET\ProjectShop\BackEnd\src\Infrastructure\Ecommerce.Infrastructure\Data\category.json");
                    var categories = JsonConvert.DeserializeObject<List<Category>>(categoriesData);
                    await context.Categories!.AddRangeAsync(categories!);
                    await context.SaveChangesAsync();
                }

                if (!context.Products!.Any())
                {
                    var productsData = File.ReadAllText(@"C:\Users\arlex\source\REPOS_.NET\ProjectShop\BackEnd\src\Infrastructure\Ecommerce.Infrastructure\Data\product.json");
                    var products = JsonConvert.DeserializeObject<List<Product>>(productsData);
                    await context.Products!.AddRangeAsync(products!);
                    await context.SaveChangesAsync();
                }

                if (!context.Images!.Any())
                {
                    var imagesData = File.ReadAllText(@"C:\Users\arlex\source\REPOS_.NET\ProjectShop\BackEnd\src\Infrastructure\Ecommerce.Infrastructure\Data\image.json");
                    var images = JsonConvert.DeserializeObject<List<Image>>(imagesData);
                    await context.Images!.AddRangeAsync(images!);
                    await context.SaveChangesAsync();
                }

                if (!context.Reviews!.Any())
                {
                    var reviewData = File.ReadAllText(@"C:\Users\arlex\source\REPOS_.NET\ProjectShop\BackEnd\src\Infrastructure\Ecommerce.Infrastructure\Data\review.json");
                    var reviews = JsonConvert.DeserializeObject<List<Review>>(reviewData);
                    await context.Reviews!.AddRangeAsync(reviews!);
                    await context.SaveChangesAsync();
                }

                if (!context.Countries!.Any())
                {
                    var countryData = File.ReadAllText(@"C:\Users\arlex\source\REPOS_.NET\ProjectShop\BackEnd\src\Infrastructure\Ecommerce.Infrastructure\Data\countries.json");
                    var countries = JsonConvert.DeserializeObject<List<Country>>(countryData);
                    await context.Countries!.AddRangeAsync(countries!);
                    await context.SaveChangesAsync();
                }

            }
            catch( Exception ex )
            {
                var logger=loggerFactory.CreateLogger<EcommerceDbContextData>();
                logger.LogError(ex.Message );
            }
        }

        //public static Task LoadDataAsync(EcommerceDbContext context, ILoggerFactory loggerFactory, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
