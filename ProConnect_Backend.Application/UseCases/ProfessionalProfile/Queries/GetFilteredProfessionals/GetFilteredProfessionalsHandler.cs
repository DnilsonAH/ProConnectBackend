using AutoMapper;
using MediatR;
using ProConnect_Backend.Application.DTOsResponse.ProfessionalProfileDTOs;
using ProConnect_Backend.Domain.Ports;
using Microsoft.Extensions.Logging;

namespace ProConnect_Backend.Application.UseCases.ProfessionalProfile.Queries.GetFilteredProfessionals;

public class GetFilteredProfessionalsHandler : IRequestHandler<GetFilteredProfessionalsQuery, PagedProfessionalResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetFilteredProfessionalsHandler> _logger;

    public GetFilteredProfessionalsHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetFilteredProfessionalsHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedProfessionalResponseDto> Handle(GetFilteredProfessionalsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando búsqueda de profesionales con filtros");
        var dto = request.FilterDto;

        //Obtener datos de la DB
        var (profiles, totalCount) = await _unitOfWork.ProfessionalProfileRepository.FilterProfilesAsync(
            dto.CategoryId,
            dto.ProfessionId,
            dto.SpecializationId,
            dto.Page,
            dto.PageSize
        );

        var resultList = new List<ProfessionalSearchResultDto>();

        //Mapeo
        foreach (var profile in profiles)
        {
            // Esto copia ID, Nombres, Foto, Presentación, etc.
            var professionalDto = _mapper.Map<ProfessionalSearchResultDto>(profile);
            // Lógica de las listas, esto se ahce manual por que depende de filtros  (category, profession, specialization)
            //Si envían Especialidad -> Devolvemos solo datos del profesional
            if (dto.SpecializationId.HasValue)
            {
            }
            //Si envían Profesión -> Mostrar solo esa profesión
            else if (dto.ProfessionId.HasValue)
            {
                var relevantSpecs = profile.ProfileSpecializations
                    .Where(ps => ps.Specialization.ProfessionId == dto.ProfessionId.Value)
                    .GroupBy(ps => ps.Specialization.Profession)
                    .Select(g => new ProfessionResultDto
                    {
                        ProfessionId = g.Key.ProfessionId,
                        ProfessionName = g.Key.ProfessionName,
                        Specializations = g.Select(s => _mapper.Map<SpecializationResultDto>(s.Specialization)).ToList()
                    });

                professionalDto.Professions.AddRange(relevantSpecs);
            }
            //Categoría o Sin Filtros
            else
            {
                var specsQuery = profile.ProfileSpecializations.AsEnumerable();

                if (dto.CategoryId.HasValue)
                {
                    specsQuery = specsQuery.Where(ps => ps.Specialization.Profession.CategoryId == dto.CategoryId.Value);
                }

                var groupedProfessions = specsQuery
                    .GroupBy(ps => ps.Specialization.Profession)
                    .Select(g => new ProfessionResultDto
                    {
                        ProfessionId = g.Key.ProfessionId,
                        ProfessionName = g.Key.ProfessionName,
                        Specializations = g.Select(s => _mapper.Map<SpecializationResultDto>(s.Specialization)).ToList()
                    });

                professionalDto.Professions.AddRange(groupedProfessions);
            }

            resultList.Add(professionalDto);
        }

        return new PagedProfessionalResponseDto
        {
            TotalCount = totalCount,
            Page = dto.Page,
            PageSize = dto.PageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)dto.PageSize),
            Professionals = resultList
        };
    }
}