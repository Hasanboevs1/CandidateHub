namespace CandidateHub.Domain.Entities;

public class Candidate
{
    public long Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty; 
    public string? PhoneNumber { get; set; }
    public DateTime? BestCallTime { get; set; }
    public string? LinkedInProfile { get; set; }
    public string? GitHubProfile { get; set; }
    public string Comment { get; set; } = string.Empty;
}