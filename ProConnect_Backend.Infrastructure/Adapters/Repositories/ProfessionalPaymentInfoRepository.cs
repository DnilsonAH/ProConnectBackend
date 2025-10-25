using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Infrastructure.Adapters.Repositories;

public class ProfessionalPaymentInfoRepository : GenericRepository<ProfessionalPaymentInfo>, IProfessionalPaymentInfoRepository
{
    public ProfessionalPaymentInfoRepository(ProConnectDbContext context) : base(context)
    {
    }
    //Agregar los metodos específicos
}
