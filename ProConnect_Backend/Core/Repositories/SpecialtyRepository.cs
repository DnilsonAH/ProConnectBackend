using ProConnect_Backend.Core.Entities;
using ProConnect_Backend.Core.Repositories.Interfaces;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Core.Repositories;

public class SpecialtyRepository : GenericRepository<Specialty>, ISpecialtyRepository
{
    public SpecialtyRepository(ProConnectDbContext context) : base(context)
    {
    }
    //Agregar los metodos específicos
}
