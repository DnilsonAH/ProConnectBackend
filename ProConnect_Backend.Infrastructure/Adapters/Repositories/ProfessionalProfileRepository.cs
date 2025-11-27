using Microsoft.EntityFrameworkCore;
using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Infrastructure.Adapters.Repositories;

public class ProfessionalProfileRepository : GenericRepository<ProfessionalProfile>, IProfessionalProfileRepository
{
    public ProfessionalProfileRepository(ProConnectDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<ProfessionalProfile?> GetByUserIdAsync(uint userId)
    {
        return await _dbContext.ProfessionalProfiles
            .Include(p => p.ProfileSpecializations)
                .ThenInclude(ps => ps.Specialization)
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task<IEnumerable<ProfessionalProfile>> GetProfilesByProfessionAsync(uint professionId)
    {
        return await _dbContext.ProfessionalProfiles
            .Where(p => p.ProfileSpecializations.Any(ps => ps.Specialization.ProfessionId == professionId))
            .Include(p => p.User)
            .Include(p => p.ProfileSpecializations)
                .ThenInclude(ps => ps.Specialization)
            .ToListAsync();
    }

    public async Task<ProfessionalProfile?> GetProfileWithDetailsAsync(uint profileId)
    {
        return await _dbContext.ProfessionalProfiles
            .Include(p => p.User)
            .Include(p => p.ProfileSpecializations)
                .ThenInclude(ps => ps.Specialization)
            .FirstOrDefaultAsync(p => p.ProfileId == profileId);
    }

    public async Task<IEnumerable<ProfessionalProfile>> GetVerifiedProfilesAsync()
    {
        return await _dbContext.ProfessionalProfiles
            .Where(p => p.UserId > 0)
            .Include(p => p.User)
            .Include(p => p.ProfileSpecializations)
                .ThenInclude(ps => ps.Specialization)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProfessionalProfile>> GetProfilesByAvailabilityAsync(DayOfWeek day)
    {
        var dayString = day.ToString().ToLower();
        return await _dbContext.ProfessionalProfiles
            .Where(p => p.User.WeeklyAvailabilities.Any(wa => wa.WeekDay == dayString && wa.IsAvailable == true))
            .Include(p => p.User)
            .Include(p => p.ProfileSpecializations)
                .ThenInclude(ps => ps.Specialization)
            .ToListAsync();
    }
    
    // Filtrado de profesionales por categoría, profesión y especialización | Tupla:(Lista de perfiles, Total de items para paginacion)
    public async Task<(IEnumerable<ProfessionalProfile> Items, int TotalCount)> FilterProfilesAsync(uint? categoryId, uint? professionId, uint? specializationId, int page, int pageSize)
    {
        // iniciacion de la consulta
        var query = _dbContext.ProfessionalProfiles
            .Include(p => p.User)
            .Include(p => p.ProfileSpecializations)
                .ThenInclude(ps => ps.Specialization)
                    .ThenInclude(s => s.Profession)
                        .ThenInclude(pr => pr.Category)
            .AsQueryable();

        // Filtro de Seguridad si el usuario tiene alguna verificación con status "Verified" o "Approved"
        query = query.Where(p => p.User.Verifications.Any(v => v.Status == "Verified" || v.Status == "Approved"));

        // Filtracion dinamica | If speciality else if profession else if Category
        if (specializationId.HasValue)
        {
            // Si busca por especialidad, el perfil debe tener esa especialidad
            query = query.Where(p => p.ProfileSpecializations.Any(ps => ps.SpecializationId == specializationId.Value));
        }
        else if (professionId.HasValue)
        {
            // Si busca por profesión, el perfil debe tener alguna especialidad de esa profesión
            query = query.Where(p => p.ProfileSpecializations.Any(ps => ps.Specialization.ProfessionId == professionId.Value));
        }
        else if (categoryId.HasValue)
        {
            // Si busca por categoría, el perfil debe tener alguna especialidad -> profesión -> categoría
            query = query.Where(p => p.ProfileSpecializations.Any(ps => ps.Specialization.Profession.CategoryId == categoryId.Value));
        }

        //Conteo total para paginación, antes de cortar la página
        var totalCount = await query.CountAsync();

        //Paginación, Skip y Take se traducen a LIMIT y OFFSET en SQL
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
