using AutoMapper;
using CandidateHub.Data.Repositories;
using CandidateHub.Domain.Entities;
using CandidateHub.Service.DTOs.Candidate;
using CandidateHub.Service.Exceptions;
using CandidateHub.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CandidateHub.Service.Services;

public class CandidateService : ICandidateService
{
    private readonly IRepository<Candidate> _candidateRepository;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;

    private const string CandidateCacheKey = "CandidatesCache";

    public CandidateService(IRepository<Candidate> candidateRepository, IMapper mapper, IMemoryCache cache)
    {
        _candidateRepository = candidateRepository;
        _mapper = mapper;
        _cache = cache;
    }

    public async ValueTask<CandidateDto> CreateCandidateAsync(CandidateCreateDto dto)
    {
        var existCandidate = await _candidateRepository
            .GetAsync(c => c.FirstName.ToLower() == dto.FirstName.ToLower() && c.Email.ToLower() == dto.Email.ToLower());
        if (existCandidate is not null)
            throw new CustomException(409, "candidate_already_exist");

        var mappedModel = _mapper.Map<Candidate>(dto);
        var createdCandidate = await _candidateRepository.CreateAsync(mappedModel);
        await _candidateRepository.SaveChangesAsync();

        _cache.Remove(CandidateCacheKey);

        return _mapper.Map<CandidateDto>(createdCandidate);
    }

    public async ValueTask<bool> DeleteCandidateAsync(long id)
    {
        var model = await _candidateRepository.GetAsync(c => c.Id == id);
        if (model is null)
            throw new CustomException(404, "candidate_not_found");

        await _candidateRepository.DeleteAsync(id);
        await _candidateRepository.SaveChangesAsync();

        _cache.Remove(CandidateCacheKey);

        return true;
    }

    public async ValueTask<IEnumerable<CandidateDto>> GetAllCandidatessAsync()
    {
        if (!_cache.TryGetValue(CandidateCacheKey, out IEnumerable<CandidateDto>? cachedCandidates))
        {
            var models = await _candidateRepository.GetAll(x => true).ToListAsync();
            cachedCandidates = _mapper.Map<IEnumerable<CandidateDto>>(models);

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(CandidateCacheKey, cachedCandidates, cacheOptions);
        }

        return cachedCandidates!;
    }

    public async ValueTask<CandidateDto> GetCandidatetAsync(long id)
    {
        var model = await _candidateRepository.GetAsync(c => c.Id == id);
        if (model is null)
            throw new CustomException(404, "candidate_not_found");

        return _mapper.Map<CandidateDto>(model);
    }

    public async ValueTask<CandidateDto> UpdateCandidateAsync(long id, CandidateUpdateDto dto)
    {
        var model = await _candidateRepository.GetAsync(c => c.Id == id);
        if (model is null)
            throw new CustomException(404, "candidate_not_found");

        _mapper.Map(dto, model);
        var result = _candidateRepository.Update(model);
        await _candidateRepository.SaveChangesAsync();

        _cache.Remove(CandidateCacheKey);

        return _mapper.Map<CandidateDto>(result);
    }
}
