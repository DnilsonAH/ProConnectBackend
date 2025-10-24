using AutoMapper;
using ProConnect_Backend.Core.Repositories.Interfaces;
using ProConnect_Backend.Core.Services.interfaces;

namespace ProConnect_Backend.Core.Services;

public class GenericService<Tentity, Tdto> : IGenericService<Tdto> where Tentity : class where Tdto : class
{
    //inyecto el repositorio y el mapper
    protected readonly IGenericRepository<Tentity> _repository;
    protected readonly IMapper _mapper;
    public GenericService(IGenericRepository<Tentity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;

    }
    
    public virtual async Task<IEnumerable<Tdto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<Tdto>>(entities);
        return dtos;
    }
    
    public virtual async Task<Tdto?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null) return null;
        var dto = _mapper.Map<Tdto>(entity);
        return dto;
    }
    public virtual async Task AddAsync(Tdto dto)
    {
        var entity = _mapper.Map<Tentity>(dto);
        _repository.AddAsync(entity);
    }
    public virtual async Task UpdateAsync(Tdto dto)
    {
        var entity = _mapper.Map<Tentity>(dto);
        _repository.UpdateAsync(entity);
    }

    public virtual async Task DeleteAsync(int id)
    {
        _repository.DeleteAsync(id);
    }

}