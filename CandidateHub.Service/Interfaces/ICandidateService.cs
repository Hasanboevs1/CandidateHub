using CandidateHub.Service.DTOs.Candidate;

namespace CandidateHub.Service.Interfaces;

public interface ICandidateService
{
    ValueTask<IEnumerable<CandidateDto>> GetAllCandidatessAsync();
    ValueTask<CandidateDto?> GetCandidatetAsync(long id);
    ValueTask<CandidateDto> CreateCandidateAsync(CandidateCreateDto dto);
    ValueTask<CandidateDto> UpdateCandidateAsync(long id, CandidateUpdateDto dto);
    ValueTask<bool> DeleteCandidateAsync(long id);
}
