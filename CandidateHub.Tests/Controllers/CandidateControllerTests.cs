using CandidateHub.Api.Controllers;
using CandidateHub.Service.DTOs.Candidate;
using CandidateHub.Service.Exceptions;
using CandidateHub.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CandidateHub.Tests.Controllers;

public class CandidateControllerTests
{
    private readonly Mock<ICandidateService> _mockCandidateService;
    private readonly CandidateController _controller;

    public CandidateControllerTests()
    {
        _mockCandidateService = new Mock<ICandidateService>();
        _controller = new CandidateController(_mockCandidateService.Object);
    }

    [Fact]
    public async Task GetAllCandidates_ReturnsOkResult_WithCandidateList()
    {
        // Arrange
        var candidates = new List<CandidateDto>
        {
            new CandidateDto { Id = 1, FirstName = "Leyla", LastName = "Mananov", Email = "leyla@markaz.uz" },
            new CandidateDto { Id = 2, FirstName = "Temur", LastName = "Abdullayev", Email = "temura500@gmail.com" }
        };

        _mockCandidateService.Setup(service => service.GetAllCandidatessAsync())
                                .ReturnsAsync(candidates);

        // Act
        var result = await _controller.GetAllCandidates();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCandidates = Assert.IsAssignableFrom<IEnumerable<CandidateDto>>(okResult.Value);
        Assert.Equal(2, returnedCandidates.Count());
    }

    [Fact]
    public async Task GetCandidate_ReturnsOkResult_WithCandidate()
    {
        // Arrange
        var candidateId = 1L;
        var candidate = new CandidateDto { Id = candidateId, FirstName = "Leyle", LastName = "Mananov", Email = "leyla@markaz.uz" };

        _mockCandidateService.Setup(service => service.GetCandidatetAsync(candidateId))
                                .ReturnsAsync(candidate);

        // Act
        var result = await _controller.GetCandidate(candidateId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCandidate = Assert.IsType<CandidateDto>(okResult.Value);
        Assert.Equal(candidateId, returnedCandidate.Id);
    }


    [Fact]
    public async Task AddCandidate_ReturnsOkResult_WithCreatedCandidate()
    {
        // Arrange
        var createDto = new CandidateCreateDto
        {
            FirstName = "Sardor",
            LastName = "Murodov",
            Email = "sardormuradov@markaz.uz"
        };

        var createdCandidate = new CandidateDto
        {
            Id = 1,
            FirstName = "Sardor",
            LastName = "Murodov",
            Email = "sardormuradov@markaz.uz"
        };

        _mockCandidateService.Setup(service => service.CreateCandidateAsync(createDto))
                                .ReturnsAsync(createdCandidate);

        // Act
        var result = await _controller.AddCandidateAsync(createDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCandidate = Assert.IsType<CandidateDto>(okResult.Value);
        Assert.Equal(createdCandidate.Id, returnedCandidate.Id);
    }


    [Fact]
    public async Task UpdateCandidate_ReturnsOkResult_WithUpdatedCandidate()
    {
        // Arrange
        var candidateId = 1L;
        var updateDto = new CandidateUpdateDto
        {
            FirstName = "Sardor",
            LastName = "Murodov",
            Email = "sardormuradov@markaz.uz"
        };

        var updatedCandidate = new CandidateDto
        {
            Id = candidateId,
            FirstName = "Sardor",
            LastName = "Murodov",
            Email = "sardormuradov@markaz.uz"
        };

        _mockCandidateService.Setup(service => service.UpdateCandidateAsync(candidateId, updateDto))
                                .ReturnsAsync(updatedCandidate);

        // Act
        var result = await _controller.UpdateCandidateAsync(candidateId, updateDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCandidate = Assert.IsType<CandidateDto>(okResult.Value);
        Assert.Equal(candidateId, returnedCandidate.Id);
    }


   
     [Fact]
        public async Task UpdateCandidate_ReturnsBadRequestResult_WhenServiceThrowsException()
        {
            // Arrange
            var candidateId = 1L;
            var updateDto = new CandidateUpdateDto
            {
                FirstName = "Sardor",
                LastName = "Murodov",
                Email = "sardormuradov@markaz.uz"
            };

            _mockCandidateService.Setup(service => service.UpdateCandidateAsync(candidateId, updateDto))
                                 .ThrowsAsync(new CustomException(400, "invalid_data"));

            // Act
            var result = await _controller.UpdateCandidateAsync(candidateId, updateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorObject = Assert.IsType<ProblemDetails>(badRequestResult.Value);
            Assert.Equal(400, errorObject.Status);
            Assert.Equal("invalid_data", errorObject.Title);
        }

        [Fact]
        public async Task UpdateCandidate_ReturnsNotFoundResult_WhenCandidateDoesNotExist()
        {
            // Arrange
            var candidateId = 1L;
            var updateDto = new CandidateUpdateDto
            {
                FirstName = "Sardor",
                LastName = "Murodov",
                Email = "sardormuradov@markaz.uz"
            };

            _mockCandidateService.Setup(service => service.UpdateCandidateAsync(candidateId, updateDto))
                                 .ThrowsAsync(new CustomException(404, "candidate_not_found"));

            // Act
            var result = await _controller.UpdateCandidateAsync(candidateId, updateDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var errorObject = Assert.IsType<ProblemDetails>(notFoundResult.Value);
            Assert.Equal(404, errorObject.Status);
            Assert.Equal("candidate_not_found", errorObject.Title);
        }
    
        
    [Fact]
    public async Task RemoveCandidate_ReturnsNoContentResult_WhenCandidateDeleted()
    {
        // Arrange
        var candidateId = 1L;

        _mockCandidateService.Setup(service => service.DeleteCandidateAsync(candidateId))
                                .ReturnsAsync(true);

        // Act
        var result = await _controller.RemoveCandidateAsync(candidateId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task RemoveCandidate_ReturnsNotFoundResult_WhenCandidateDoesNotExist()
    {
        // Arrange
        var candidateId = 1L;

        _mockCandidateService.Setup(service => service.DeleteCandidateAsync(candidateId))
                                .ReturnsAsync(false);

        // Act
        var result = await _controller.RemoveCandidateAsync(candidateId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
