using AutoMapper;
using CandidateHub.Data.Repositories;
using CandidateHub.Domain.Entities;
using CandidateHub.Service.DTOs.Candidate;
using CandidateHub.Service.Interfaces;

namespace CandidateHub.Service.Services;

public class CandidateService : ICandidateService
{
    private readonly IRepository<Candidate> _candidateRepository;
    private readonly IMapper _mapper;
    public CandidateService(IRepository<Candidate> candidateRepository, IMapper mapper) =>
        (_candidateRepository, _mapper) = (candidateRepository, mapper);


    public async ValueTask<CandidateDto> CreateProductAsync(CandidateCreateDto product)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<bool> DeleteProductAsync(long id)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<CandidateDto?> GetProductAsync(long id)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<IEnumerable<CandidateDto>> GetProductsAsync()
    {
        throw new NotImplementedException();
    }

    public async ValueTask<CandidateDto> UpdateProductAsync(CandidateUpdateDto product)
    {
        throw new NotImplementedException();
    }
}
