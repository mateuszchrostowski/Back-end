using ASPNET23.Model;
using ASPNET23.Model.Entities;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNET23.Repository.Categories
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await Context.Categories
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Category>> GetBySearchText(string text)
        {
            text = text.ToLower();
            return await Context.Categories
                .Where(x => x.Name.ToLower().Contains(text))
                .ToListAsync();
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await Context.Categories
                .Include(x => x.Products)
                .ToListAsync();
        }

        public async Task<bool> SaveCategoryAsync(Category category)
        {
            if (category == null)
                return false;

            Context.Entry(category).State = category.Id == default(int) ? EntityState.Added : EntityState.Modified;

            try
            {
                await Context.SaveChangesAsync();
            }
            catch(Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await GetByIdAsync(id);
            if (category == null)
                return true;

            Context.Categories.Remove(category);

            try
            {
                await Context.SaveChangesAsync();
            }
            catch(Exception)
            {
                return false;
            }

            return true;
        }
    }
}
