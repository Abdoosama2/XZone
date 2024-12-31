using XZone.Data;
using XZone.Models;
using XZone.Repository.IRepository;

namespace XZone.Repository
{
    public class GameRepository : Repository<Game>,IGameRepository
    {
        public GameRepository(AppDbContext context) : base(context)
        {
        }


    }
}
