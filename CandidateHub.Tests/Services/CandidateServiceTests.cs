using AutoMapper;
using CandidateHub.Data.Repositories;
using CandidateHub.Domain.Entities;
using CandidateHub.Service.DTOs.Candidate;
using CandidateHub.Service.Exceptions;
using CandidateHub.Service.Services;
using Moq;
using System.Linq.Expressions;

namespace CandidateHub.Service.Tests;

public class CandidateServiceTests
{
    private readonly Mock<IRepository<Candidate>> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CandidateService _candidateService;

    public CandidateServiceTests()
    {
        _mockRepository = new Mock<IRepository<Candidate>>();
        _mockMapper = new Mock<IMapper>();
        _candidateService = new CandidateService(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreateCandidateAsync_ShouldCreateCandidate_WhenCandidateDoesNotExist()
    {
        // Arrange
        var createDto = new CandidateCreateDto
        {
            FirstName = "Hurshid",
            LastName = "Aliev",
            Email = "hurshid@note.uz"
        };

        var candidate = new Candidate
        {
            Id = 3,
            FirstName = "Hurshid",
            LastName = "Aliev",
            Email = "hurshid@note.uz"
        };

        _mockRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Candidate, bool>>>(), true, null))
                       .ReturnsAsync((Candidate?)null);

        _mockMapper.Setup(mapper => mapper.Map<Candidate>(createDto))
                   .Returns(candidate);

        _mockRepository.Setup(repo => repo.CreateAsync(candidate))
                       .ReturnsAsync(candidate);

        _mockMapper.Setup(mapper => mapper.Map<CandidateDto>(candidate))
                   .Returns(new CandidateDto());

        // Act
        var result = await _candidateService.CreateCandidateAsync(createDto);

        // Assert
        _mockRepository.Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<Candidate, bool>>>(), true, null), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<Candidate>(createDto), Times.Once);
        _mockRepository.Verify(repo => repo.CreateAsync(candidate), Times.Once);
        _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<CandidateDto>(candidate), Times.Once);
    }

    [Fact]
    public async Task CreateCandidateAsync_ShouldThrowException_WhenCandidateExists()
    {
        // Arrange
        var createDto = new CandidateCreateDto
        {
            FirstName = "Hurshid",
            LastName = "Aliev",
            Email = "hurshid@note.uz"
        };

        var existingCandidate = new Candidate
        {
            Id = 3,
            FirstName = "Hurshid",
            LastName = "Aliev",
            Email = "hurshid@note.uz"
        };

        _mockRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Candidate, bool>>>(), true, null))
                       .ReturnsAsync(existingCandidate);

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(async () => await _candidateService.CreateCandidateAsync(createDto));

        _mockRepository.Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<Candidate, bool>>>(), true, null), Times.Once);
    }

    [Fact]
    public async Task DeleteCandidateAsync_ShouldDeleteCandidate_WhenCandidateExists()
    {
        // Arrange
        var candidateId = 3L;

        var candidate = new Candidate
        {
            Id = candidateId,
            FirstName = "Hurshid",
            LastName = "Aliev",
            Email = "hurshid@note.uz"
        };

        _mockRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Candidate, bool>>>(), true, null))
                       .ReturnsAsync(candidate);

        _mockRepository.Setup(repo => repo.DeleteAsync(candidateId))
                       .ReturnsAsync(true);

        // Act
        var result = await _candidateService.DeleteCandidateAsync(candidateId);

        // Assert
        Assert.True(result);
        _mockRepository.Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<Candidate, bool>>>(), true, null), Times.Once);
        _mockRepository.Verify(repo => repo.DeleteAsync(candidateId), Times.Once);
        _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteCandidateAsync_ShouldThrowException_WhenCandidateDoesNotExist()
    {
        // Arrange
        var candidateId = 3L;

        _mockRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Candidate, bool>>>(), true, null))
                       .ReturnsAsync((Candidate?)null);

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(async () => await _candidateService.DeleteCandidateAsync(candidateId));

        _mockRepository.Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<Candidate, bool>>>(), true, null), Times.Once);
    }


    [Fact]
    public async Task GetCandidateAsync_ShouldReturnCandidate_WhenCandidateExists()
    {
        // Arrange
        var candidateId = 3L;

        var candidate = new Candidate
        {
            Id = candidateId,
            FirstName = "Hurshid",
            LastName = "Aliev",
            Email = "hurshid@note.uz"
        };

        _mockRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Candidate, bool>>>(), true, null))
                       .ReturnsAsync(candidate);

        _mockMapper.Setup(mapper => mapper.Map<CandidateDto>(candidate))
                   .Returns(new CandidateDto());

        // Act
        var result = await _candidateService.GetCandidatetAsync(candidateId);

        // Assert
        _mockRepository.Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<Candidate, bool>>>(), true, null), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<CandidateDto>(candidate), Times.Once);
    }

    [Fact]
    public async Task GetCandidateAsync_ShouldThrowException_WhenCandidateDoesNotExist()
    {
        // Arrange
        var candidateId = 3L;

        _mockRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Candidate, bool>>>(), true, null))
                       .ReturnsAsync((Candidate?)null);

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(async () => await _candidateService.GetCandidatetAsync(candidateId));

        _mockRepository.Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<Candidate, bool>>>(), true, null), Times.Once);
    }

    [Fact]
    public async Task UpdateCandidateAsync_ShouldUpdateCandidate_WhenCandidateExists()
    {
        // Arrange
        var candidateId = 3L;

        var updateDto = new CandidateUpdateDto
        {
            FirstName = "Hurshid",
            LastName = "Aliev",
            Email = "hurshid@note.uz"
        };

        var candidate = new Candidate
        {
            Id = candidateId,
            FirstName = "Hurshid",
            LastName = "Aliev",
            Email = "hurshid@note.uz"
        };

        _mockRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Candidate, bool>>>(), true, null))
                       .ReturnsAsync(candidate);

        _mockMapper.Setup(mapper => mapper.Map(updateDto, candidate))
                   .Verifiable();

        _mockRepository.Setup(repo => repo.Update(candidate))
                       .Returns(candidate);

        _mockMapper.Setup(mapper => mapper.Map<CandidateDto>(candidate))
                   .Returns(new CandidateDto());

        // Act
        var result = await _candidateService.UpdateCandidateAsync(candidateId, updateDto);

        // Assert
        _mockRepository.Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<Candidate, bool>>>(), true, null), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map(updateDto, candidate), Times.Once);
        _mockRepository.Verify(repo => repo.Update(candidate), Times.Once);
        _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<CandidateDto>(candidate), Times.Once);
    }

    [Fact]
    public async Task UpdateCandidateAsync_ShouldThrowException_WhenCandidateDoesNotExist()
    {
        // Arrange
        var candidateId = 3L;

        var updateDto = new CandidateUpdateDto
        {
            FirstName = "Hurshid",
            LastName = "Aliev",
            Email = "hurshid@note.uz"
        };

        _mockRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Candidate, bool>>>(), true, null))
                       .ReturnsAsync((Candidate?)null);

        // Act & Assert
        await Assert.ThrowsAsync<CustomException>(async () => await _candidateService.UpdateCandidateAsync(candidateId, updateDto));

        _mockRepository.Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<Candidate, bool>>>(), true, null), Times.Once);
    }
}
