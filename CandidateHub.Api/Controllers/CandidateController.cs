using CandidateHub.Domain.Entities;
using CandidateHub.Service.DTOs.Candidate;
using CandidateHub.Service.Exceptions;
using CandidateHub.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace CandidateHub.Api.Controllers;
/// <summary>
/// Controller for managing candidate-related operations.
/// </summary>
[Route("api/candidates")]
[ApiController]
public class CandidateController : ControllerBase
{
    private readonly ICandidateService _candidateService;
    public CandidateController(ICandidateService candidateService) 
        => _candidateService = candidateService;


    /// <summary>
    /// Retrieves a list of all candidates.
    /// </summary>
    /// <remarks>
    /// This endpoint returns a collection of all candidates stored in the system.
    /// </remarks>
    /// <response code="200">Returns the list of candidates.</response>
    /// <response code="500">If there is an internal server error.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Candidate>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllCandidates()
        => Ok(await _candidateService.GetAllCandidatessAsync());

    /// <summary>
    /// Retrieves a candidate by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the candidate.</param>
    /// <response code="200">Returns the candidate with the specified ID.</response>
    /// <response code="404">If no candidate is found with the given ID.</response>
    /// <response code="500">If there is an internal server error.</response>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(Candidate), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCandidate(long id)
        => Ok(await _candidateService.GetCandidatetAsync(id));

    /// <summary>
    /// Adds a new candidate to the system.
    /// </summary>
    /// <param name="dto">The data required to create a new candidate.</param>
    /// <response code="200">Returns the newly created candidate.</response>
    /// <response code="400">If the provided data is invalid.</response>
    /// <response code="500">If there is an internal server error.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Candidate), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddCandidateAsync([FromBody] CandidateCreateDto dto)
        => Ok(await _candidateService.CreateCandidateAsync(dto));

    /// <summary>
    /// Updates an existing candidate's information.
    /// </summary>
    /// <param name="id">The unique identifier of the candidate to update.</param>
    /// <param name="dto">The updated data for the candidate.</param>
    /// <response code="200">Returns the updated candidate.</response>
    /// <response code="400">If the provided data is invalid.</response>
    /// <response code="404">If no candidate is found with the given ID.</response>
    /// <response code="500">If there is an internal server error.</response>
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(CandidateDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateCandidateAsync(long id, [FromBody] CandidateUpdateDto dto)
    {
        try
        {
            var result = await _candidateService.UpdateCandidateAsync(id, dto);
            return Ok(result);
        }
        catch (CustomException ex)
        {
            if (ex.StatusCode == 400)
            {
                return BadRequest(new ProblemDetails { Status = ex.StatusCode, Title = ex.Message });
            }
            else if (ex.StatusCode == 404)
            {
                return NotFound(new ProblemDetails { Status = ex.StatusCode, Title = ex.Message });
            }
            else
            {
                return StatusCode(ex.StatusCode, new ProblemDetails { Status = ex.StatusCode, Title = ex.Message });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ProblemDetails { Status = 500, Title = "Internal Server Error" });
        }
    }

    /// <summary>
    /// Deletes a candidate from the system.
    /// </summary>
    /// <param name="id">The unique identifier of the candidate to delete.</param>
    /// <response code="204">If the candidate is successfully deleted.</response>
    /// <response code="404">If no candidate is found with the given ID.</response>
    /// <response code="500">If there is an internal server error.</response>
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveCandidateAsync(long id)
        => await _candidateService.DeleteCandidateAsync(id) ? NoContent() : NotFound();
}
