using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DealerTrack.DataBase;
using DealerTrack.DataBase.Entities;
using DealerTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace DealerTrack.Services
{
    public interface IBaseReadCreateUpdateService<TDto> : IBaseCreateUpdateService<TDto>
    {
        Task<List<TDto>> GetAllAsync();
    }
    public class BaseReadCreateUpdateService<TEntity, TDto> : BaseCreateUpdateService<TEntity, TDto>, IBaseReadCreateUpdateService<TDto>
        where TEntity : BaseEntity, new()
        where TDto : BaseDto, new()
    {
        public BaseReadCreateUpdateService(DealerTrackDbContext dealerTrackDbContext) : base(dealerTrackDbContext)
        {
        }

        

        public async Task<List<TDto>> GetAllAsync()
        {
            return await DealerTrackDbContext.Set<TEntity>().AsNoTracking()
                .Select(ProjectEntityToDto())
                .ToListAsync();
        }

        protected virtual Expression<Func<TEntity, TDto>> ProjectEntityToDto()
        {
            return entity => new TDto
            {
                Id = entity.Id
            };
        }
    }
}