using ASPNET23.Model.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNET23.Model
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using(var context = new AppDbContext(serviceProvider
                .GetRequiredService<DbContextOptions<AppDbContext>>())) { 

                if(context.Categories.Any())
                {
                    return;
                }

                context.Categories.AddRange(
                    new Entities.Category()
                    {
                        Name = "Rowery"
                    },
                    new Entities.Category()
                    {
                        Name = "Samochody"
                    }
                    );

                context.SaveChanges();

                var categories = 
                    context.Categories;

                context.Products.AddRange(
                    new Product()
                    {
                        Name = "Składak",
                        Price = 50,
                        Description = "",
                        CategoryId = categories.Single(x => x.Name == "Rowery").Id
                    },
                    new Product()
                    {
                        Name = "Bus",
                        Price = 100,
                        Description = "",
                        CategoryId = categories.Single(x => x.Name == "Samochody").Id
                    },
                    new Product()
                    {
                        Name = "Kabriolet",
                        Price = 150,
                        Description = "",
                        CategoryId = categories.Single(x => x.Name == "Samochody").Id
                    }
                    );

                context.SaveChanges();
            }
        }
    }
}
