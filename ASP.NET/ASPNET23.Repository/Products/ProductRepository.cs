using ASPNET23.Model;
using ASPNET23.Model.Entities;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNET23.Repository.Products
{
    public class ProductRepository : BaseRepository, IProductsRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await Context.Products
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await Context.Products
                .ToListAsync();
        }

        public async Task<bool> SaveProductAsync(Product product)
        {
            if (product == null)
                return false;

            Context.Entry(product).State = product.Id == default(int) ? EntityState.Added : EntityState.Modified;

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await GetByIdAsync(id);
            if (product == null)
                return true;

            Context.Products.Remove(product);

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
