using System.Threading.Tasks;
using DealerTrack.DataBase;
using DealerTrack.DataBase.Entities;
using DealerTrack.Models.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace DealerTrack.Services
{
    public interface IVehicleService : IBaseCreateUpdateService<VehicleDto>
    {
        Task<Vehicle> GetEntityByName(string name);
    }
    public class VehicleService : BaseCreateUpdateService<Vehicle, VehicleDto>, IVehicleService
    {
        public VehicleService(DealerTrackDbContext dealerTrackDbContext) : base(dealerTrackDbContext)
        {
        }

        protected override Vehicle ProjectToEntity(VehicleDto dto)
        {
            return new Vehicle
            {
                Id = dto.Id ?? 0,
                Name = dto.Name
            };
        }

        public async Task<Vehicle> GetEntityByName(string name)
        {
            return await DealerTrackDbContext.Vehicles.FirstOrDefaultAsync(q => q.Name == name);
        }
    }
}