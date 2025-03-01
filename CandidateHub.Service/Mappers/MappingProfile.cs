using AutoMapper;
using CandidateHub.Domain.Entities;
using CandidateHub.Service.DTOs.Candidate;

namespace CandidateHub.Service.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile() 
    {
        CreateMap<Candidate, CandidateDto>().ReverseMap();
        CreateMap<Candidate, CandidateCreateDto>().ReverseMap();
        CreateMap<Candidate, CandidateUpdateDto>().ReverseMap();
    }
}
