using XZone.Data;
using XZone.Models;
using XZone.Repository.IRepository;

namespace XZone.Repository
{
    public class DeviceRepository : Repository<Device>, IDeviceRepository
    {
        public DeviceRepository(AppDbContext context) : base(context)
        {
        }

    }
}
