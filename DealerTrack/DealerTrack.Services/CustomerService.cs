using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DealerTrack.DataBase;
using DealerTrack.DataBase.Entities;
using DealerTrack.Models.Customers;
using Microsoft.EntityFrameworkCore;

namespace DealerTrack.Services
{
    public interface ICustomerService : IBaseCreateUpdateService<CustomerDto>
    {
        Task<Customer> GetEntityByName(string name);
    }
    public class CustomerService: BaseCreateUpdateService<Customer, CustomerDto>, ICustomerService
    {
        public CustomerService(DealerTrackDbContext dealerTrackDbContext) : base(dealerTrackDbContext)
        {
        }

        protected override Customer ProjectToEntity(CustomerDto dto)
        {
            return new Customer
            {
                Id = dto.Id ?? 0,
                Name = dto.Name
            };
        }

        public async Task<Customer> GetEntityByName(string name)
        {
            return await DealerTrackDbContext.Customers.FirstOrDefaultAsync(q => q.Name == name);
        }
    }
}