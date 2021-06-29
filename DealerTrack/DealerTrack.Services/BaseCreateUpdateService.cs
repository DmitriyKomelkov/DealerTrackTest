using System;
using System.Threading.Tasks;
using DealerTrack.DataBase;
using DealerTrack.DataBase.Entities;
using DealerTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace DealerTrack.Services
{
    public interface IBaseCreateUpdateService<TDto>
    {
        Task<TDto> SaveAsync(TDto dto);
    }
    public class BaseCreateUpdateService<TEntity, TDto>: BaseService, IBaseCreateUpdateService<TDto>
        where TEntity : BaseEntity, new()
        where TDto : BaseDto, new()
    {
        public BaseCreateUpdateService(DealerTrackDbContext dealerTrackDbContext) : base(dealerTrackDbContext)
        {
        }

        public async Task<TDto> SaveAsync(TDto dto)
        {
            TEntity entity;

            if (dto.Id == null)
            {
                entity = ProjectToEntity(dto);

                await DealerTrackDbContext.Set<TEntity>().AddAsync(entity);
            }
            else
            {
                entity = await DealerTrackDbContext.Set<TEntity>().FirstOrDefaultAsync(q => q.Id == dto.Id);
                if (entity == null)
                {
                    new ArgumentException($"Can not update {typeof(TEntity)} with Id = {dto.Id}");
                    return null;
                }

                entity = ProjectToEntity(dto);
            }

            await DealerTrackDbContext.SaveChangesAsync();

            dto.Id = entity.Id;

            return dto;
        }

        protected virtual TEntity ProjectToEntity(TDto dto)
        {
            return new TEntity();
        }
    }
}