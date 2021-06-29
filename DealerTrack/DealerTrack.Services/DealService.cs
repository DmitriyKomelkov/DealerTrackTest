using System;
using System.Linq.Expressions;
using DealerTrack.DataBase;
using DealerTrack.DataBase.Entities;
using DealerTrack.Models.Deals;

namespace DealerTrack.Services
{
    public interface IDealService: IBaseReadCreateUpdateService<DealDto>
    {

    }
    public class DealService: BaseReadCreateUpdateService<Deal, DealDto>, IDealService
    {
        public DealService(DealerTrackDbContext dealerTrackDbContext) : base(dealerTrackDbContext)
        {
        }

        protected override Deal ProjectToEntity(DealDto dto)
        {
            return new Deal
            {
                Id = dto.Id ?? 0,
                CustomerId = dto.CustomerId,
                DealNumber = dto.DealNumber,
                DealershipId = dto.DealershipId,
                VehicleId = dto.VehicleId,
                Date = dto.Date,
                Price = dto.Price
            };
        }

        protected override Expression<Func<Deal, DealDto>> ProjectEntityToDto()
        {
            return q => new DealDto
            {
                Id = q.Id,
                CustomerId = q.CustomerId,
                CustomerName = q.Customer.Name,
                DealershipId = q.DealershipId,
                DealershipName = q.Dealership.Name,
                VehicleId = q.VehicleId,
                VehicleName = q.Vehicle.Name,
                Date = q.Date,
                DealNumber = q.DealNumber,
                Price = q.Price
            };
        }
    }
}