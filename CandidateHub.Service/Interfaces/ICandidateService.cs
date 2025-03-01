using CandidateHub.Service.DTOs.Candidate;

namespace CandidateHub.Service.Interfaces;

public interface ICandidateService
{
    ValueTask<IEnumerable<CandidateDto>> GetProductsAsync();
    ValueTask<CandidateDto?> GetProductAsync(long id);
    ValueTask<CandidateDto> CreateProductAsync(CandidateCreateDto product);
    ValueTask<CandidateDto> UpdateProductAsync(CandidateUpdateDto product);
    ValueTask<bool> DeleteProductAsync(long id);
}
