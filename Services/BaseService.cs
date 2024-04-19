using AutoMapper;
using EnergyEfficiencyBE.Dtos;
using EnergyEfficiencyBE.Models.Entities;
using EnergyEfficiencyBE.Models.ResultPattern;
using EnergyEfficiencyBE.Repositories;
using System.Linq.Expressions;

namespace EnergyEfficiencyBE.Services
{
    public class BaseService<TEntity, TDto> : IBaseService<TEntity, TDto>
    where TEntity : class, IEntity, new()
    where TDto : BaseDto
    {
        protected readonly IBaseRepository<TEntity> Repository;
        protected readonly IMapper Mapper;

        public BaseService(IBaseRepository<TEntity> repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        public virtual async Task<Result<List<TEntity>?>> FindAllAsync()
        {
            try
            {
                var entities = await Repository.FindAllAsync();
                if (!entities.Any())
                {
                    return Result.Ok<List<TEntity>?>(entities);
                }

                return Result.Ok<List<TEntity>?>(entities);
            }
            catch (Exception ex)
            {
                return Result.Fail<List<TEntity>?>("BaseService Server Fail!");
            }
        }

        public virtual async Task<Result<TEntity?>> FindByIdAsync(Guid id)
        {
            try
            {
                var entity = await Repository.FindByIdAsync(id);
                if (entity == null)
                {
                    return Result.Fail<TEntity?>($"{typeof(TEntity).Name} not found.");
                }

                return Result.Ok<TEntity?>(entity);
            }
            catch (Exception ex)
            {
                return Result.Fail<TEntity?>("BaseService Server Fail!");
            }
        }

        public virtual async Task<Result<List<TEntity>?>> FindByConditionAsync(Expression<Func<TEntity, bool>> expression)
        {
            try
            {
                var entities = await Repository.FindByConditionAsync(expression);
                if (!entities.Any())
                {
                    return Result.Ok<List<TEntity>?>(entities);
                }

                return Result.Ok<List<TEntity>?>(entities);
            }
            catch (Exception ex)
            {
                return Result.Fail<List<TEntity>?>("BaseService Server Fail!");
            }
        }

        public virtual async Task<Result> CreateAsync(TDto dto)
        {
            try
            {
                var entity = new TEntity();
                Mapper.Map(dto, entity);
                dto.Id = entity.Id;

                await Repository.CreateAsync(entity);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail("BaseService Server Fail!");
            }
        }

        public virtual async Task<Result> UpdateAsync(TDto dto)
        {
            try
            {
                var entity = await Repository.FindByIdAsync(dto.Id);

                Mapper.Map(dto, entity);

                await Repository.UpdateAsync(entity);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail<TEntity>("BaseService Server Fail!");
            }
        }

        public virtual async Task<Result> DeleteAsync(Guid id)
        {
            try
            {
                var entity = await Repository.FindByIdAsync(id);
                if (entity != null)
                {
                    await Repository.DeleteAsync(entity);
                    return Result.Ok();
                }
                else
                {
                    return Result.Fail($"{typeof(TEntity).Name} not found!");
                }
            }
            catch (Exception ex)
            {
                return Result.Fail("BaseService Server Fail!");
            }
        }
    }
}
