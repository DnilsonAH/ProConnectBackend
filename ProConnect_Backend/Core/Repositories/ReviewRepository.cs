using ProConnect_Backend.Core.Entities;
using ProConnect_Backend.Core.Repositories.Interfaces;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Core.Repositories;

public class ReviewRepository : GenericRepository<Review>, IReviewRepository
{
    public ReviewRepository(ProConnectDbContext context) : base(context)
    {
    }
    //Agregar los metodos específicos
}
