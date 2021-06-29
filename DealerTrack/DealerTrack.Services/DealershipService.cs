using System.Threading.Tasks;
using DealerTrack.DataBase;
using DealerTrack.DataBase.Entities;
using DealerTrack.Models.Dealerships;
using Microsoft.EntityFrameworkCore;

namespace DealerTrack.Services
{
    public interface IDealershipService : IBaseCreateUpdateService<DealershipDto>
    {
        Task<Dealership> GetEntityByName(string name);
    }

    public class DealershipService: BaseCreateUpdateService<Dealership, DealershipDto>, IDealershipService
    {
        public DealershipService(DealerTrackDbContext dealerTrackDbContext) : base(dealerTrackDbContext)
        {
        }

        protected override Dealership ProjectToEntity(DealershipDto dto)
        {
            return new Dealership
            {
                Id = dto.Id ?? 0,
                Name = dto.Name
            };
        }

        public async Task<Dealership> GetEntityByName(string name)
        {
            return await DealerTrackDbContext.Dealerships.FirstOrDefaultAsync(q => q.Name == name);
        }
    }
}