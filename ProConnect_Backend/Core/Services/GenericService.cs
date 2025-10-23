using AutoMapper;
using ProConnect_Backend.Core.Repositories.Interfaces;
using ProConnect_Backend.Core.Services.interfaces;

namespace ProConnect_Backend.Core.Services;

public class GenericService<Tdto>: IGenericService<Tdto> where Tdto : class
{
    //Podemos usar UnitOfWork aqui en ves de repositorio per de momento dejemoslo asi
    //inyeccion de repositorio generico y mapper (el mapper neeita que la dto de la entidad exista)
    protected readonly IGenericRepository<Tdto> _repository;
    
    protected readonly IMapper _mapper;
    
    public GenericService(IGenericRepository<Tdto> repository)
    {
        _repository = repository;
    }
    
    
    public Task AddAsync(Tdto dto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Tdto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Tdto?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Tdto dto)
    {
        throw new NotImplementedException();
    }
}