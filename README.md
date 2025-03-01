🚀 Job Candidate Hub API - ASP.NET Core Web API 8

📌 Project Overview

CandidateHub(formerly known as Jobly) is a scalable, high-performance ASP.NET Core Web API 8 application designed for a seamless experience. It follows modern development principles, including SOLID, Repository Pattern, Unit Testing, Rate Limiting, Caching, and Health Checks, ensuring maintainability and efficiency.

📂 Architecture & Best Practices

1️⃣ SOLID Principles

✅ Single Responsibility Principle - Each class and method has a single responsibility.
✅ Open/Closed Principle - The system allows easy extension without modifying existing code.
✅ Liskov Substitution Principle - Derived classes can be used in place of their base classes without causing errors.
✅ Interface Segregation Principle - Interfaces are split based on usage to avoid unnecessary dependencies.
✅ Dependency Inversion Principle - Dependencies are injected via interfaces, making the system loosely coupled.

2️⃣ Repository Design Pattern

We use the Generic Repository Pattern to separate business logic from data access.

Repositories manage database operations.

Services contain business logic.

Controllers handle API requests.

Example:

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}

📦 Features Implemented

1️⃣ API Endpoints

Candidates: Create, Update, Delete, Get

2️⃣ Database Strategy

Entity Framework Core with Code First Migrations

Seeding Data using IEntityTypeConfiguration


3️⃣ Performance Optimization

✅ Rate Limiting - Prevents API abuse using built-in ASP.NET Rate Limiter.
✅ Caching - Response caching using IMemoryCache.
✅ Asynchronous Programming - Ensures scalability with async/await.

4️⃣ Unit Testing & Integration Tests

✅ XUnit, Moq, FluentAssertions for unit testing services and repositories.

Example:

public class PostServiceTests
{
    private readonly Mock<IRepository<Post>> _postRepoMock;
    private readonly PostService _postService;

    public PostServiceTests()
    {
        _postRepoMock = new Mock<IRepository<Post>>();
        _postService = new PostService(_postRepoMock.Object);
    }

    [Fact]
    public async Task GetAllPosts_ShouldReturnPosts()
    {
        var posts = new List<Post> { new() { Id = 1, Title = "Test" } };
        _postRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(posts);
        var result = await _postService.GetAllAsync();
        Assert.Equal(1, result.Count());
    }
}

5️⃣ Health Checks & Logging

✅ Health Checks - AspNetCore.HealthChecks for monitoring database & API status.
✅ Serilog - Logs structured data in files or databases.

Example:

app.UseHealthChecks("/health");

6️⃣ Self-Deploying Mechanism

✅ Docker - Containerized API using Docker.

Example Dockerfile:

FROM mcr.microsoft.com/dotnet/aspnet:8.0
COPY . /app
WORKDIR /app
EXPOSE 80

🛠️ Installation & Setup

Prerequisites

.NET 8 SDK

SQLITE

Docker (Optional for containerization)

Setup Steps

1️⃣ Clone the repository:

git clone https://github.com/Hasanboevs1/CandidateHub.git && cd CandidateHub

2️⃣ Install dependencies:

dotnet restore

3️⃣ Configure database:

dotnet ef database update

4️⃣ Run the project:

dotnet run

🎯 Git & Commit Strategy

✅ Main Branch - Stable Production-Ready Code.
✅ Pull Requests - Code reviews before merging.

Example Commit Message:

git commit -m "✨ Added rate limiting to prevent abuse"

📜 API Documentation

Swagger is enabled for easy API testing:

app.UseSwagger();

Visit: http://localhost:5000/swagger

📌 Conclusion

This project follows clean architecture, best practices, and industry standards for a robust blogging platform. 🚀

💡 Feel free to contribute or suggest improvements!

🤝 Contributing

1️⃣ Fork the repository.
2️⃣ Create a feature branch.
3️⃣ Make changes and push.
4️⃣ Create a pull request.
