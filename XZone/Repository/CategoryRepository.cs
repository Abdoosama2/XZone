using XZone.Data;
using XZone.Models;
using XZone.Models.DTO;
using XZone.Repository.IRepository;

namespace XZone.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
    }
}
