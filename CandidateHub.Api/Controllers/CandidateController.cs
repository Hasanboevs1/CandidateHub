using CandidateHub.Service.DTOs.Candidate;
using CandidateHub.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace CandidateHub.Api.Controllers;

[Route("api/candidates")]
[ApiController]
public class CandidateController : ControllerBase
{
    private readonly ICandidateService _candidateService;
    public CandidateController(ICandidateService candidateService) 
        => _candidateService = candidateService;

    [HttpGet]
    public async Task<IActionResult> GetAllCandidates() 
        => Ok( await _candidateService.GetAllCandidatessAsync());

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetCandidate(long id)
        => Ok(await _candidateService.GetCandidatetAsync(id));

    [HttpPost]
    public async Task<IActionResult> AddCandidateAsync([FromBody] CandidateCreateDto dto)
        => Ok(await _candidateService.CreateCandidateAsync(dto));

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateCandidateAsync(long id, [FromBody]CandidateUpdateDto dto)
        => Ok(await _candidateService.UpdateCandidateAsync(id, dto));

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> RemoveCandidateAsync(long id)
    => await _candidateService.DeleteCandidateAsync(id) ? NoContent() : NotFound();
}
