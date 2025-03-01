using CandidateHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CandidateHub.Data.Contexts;

internal class CandidareConfigurations : IEntityTypeConfiguration<Candidate>
{
    public void Configure(EntityTypeBuilder<Candidate> builder)
    {
        var data = new Candidate
        {
            Id = 1,
            FirstName = "Muhammadyusuf",
            LastName = "Hasanboyev",
            Email = "hasanboevs@icloud.com",
            PhoneNumber = "+998975949511",
            BestCallTime = DateTime.UtcNow.AddDays(1),
            LinkedInProfile = "https://uz.linkedin.com/in/hasanboevs",
            GitHubProfile = "https://github.com/hasanboevs1",
            Comment = "I am open to Work"
        };

        builder.HasData(data);
    }
}
