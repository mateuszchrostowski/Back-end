using ASPNET23.Model;

namespace ASPNET23.Repository
{
    public abstract class BaseRepository
    {
        protected AppDbContext Context;

        public BaseRepository(AppDbContext context)
        {
            Context = context;
        }
    }
}