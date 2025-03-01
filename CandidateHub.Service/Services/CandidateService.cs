using AutoMapper;
using CandidateHub.Data.Repositories;
using CandidateHub.Domain.Entities;
using CandidateHub.Service.DTOs.Candidate;
using CandidateHub.Service.Exceptions;
using CandidateHub.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CandidateHub.Service.Services;

public class CandidateService : ICandidateService
{
    private readonly IRepository<Candidate> _candidateRepository;
    private readonly IMapper _mapper;
    public CandidateService(IRepository<Candidate> candidateRepository, IMapper mapper) =>
        (_candidateRepository, _mapper) = (candidateRepository, mapper);

    public async ValueTask<CandidateDto> CreateCandidateAsync(CandidateCreateDto dto)
    {
        var model = await _candidateRepository.GetAll(null!)
            .FirstOrDefaultAsync(x => x.Email.ToLower() == dto.Email.ToLower());

        if (model != null)
            throw new CustomException(409, "candidate_already_exist");

        var mappedModel = _mapper.Map<Candidate>(model);
        var result = await _candidateRepository.CreateAsync(mappedModel);

        return _mapper.Map<CandidateDto>(result);
    }

    public async ValueTask<bool> DeleteCandidateAsync(long id)
    {
        var model = await _candidateRepository.GetAsync(x => x.Id == id);
        if (model is null)
            throw new CustomException(404, "candidate_not_found");

        await _candidateRepository.DeleteAsync(id);
        return true;
    }

    public async ValueTask<IEnumerable<CandidateDto>> GetAllCandidatessAsync()
    {
        var models = await _candidateRepository.GetAll(null!).ToListAsync();
        return _mapper.Map<IEnumerable<CandidateDto>>(models);
    }

    public async ValueTask<CandidateDto?> GetCandidatetAsync(long id)
    {
        var model = await _candidateRepository.GetAsync(x =>  id == x.Id);
        if (model is null)
            throw new CustomException(404, "candidate_not_found");

        return _mapper.Map<CandidateDto>(model);
    }

    public async ValueTask<CandidateDto> UpdateCandidateAsync(CandidateUpdateDto dto)
    {
        var model = await _candidateRepository.GetAsync(x => x.Id == dto.Id);
        if (model is null)
            throw new CustomException(404, "candidate_not_found");

        var mappedModel = _mapper.Map<Candidate>(dto);
        var result =  _candidateRepository.Update(mappedModel);

        return _mapper.Map<CandidateDto>(result);
    }
}
