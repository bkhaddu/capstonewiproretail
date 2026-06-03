using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RetailOptimizationPlatform.Pages
{
    [Authorize(Roles = "Admin")]
    public class ProjectSummaryModel : PageModel
    {
        public IReadOnlyList<RubricItem> RubricItems { get; } =
        [
            new("Backend and MVC", 25, "MVC controllers, Razor views, Razor Page summary, TagHelpers, routing, OOP models, SOLID service/repository boundaries."),
            new("Database and Data Access", 20, "EF Core migrations, SQL Server schema, stock audit trigger, repository pattern, and ADO.NET sales summary query."),
            new("Web API and Security", 15, "JWT-secured REST APIs, cookie admin login, Admin role authorization, model validation, and anti-forgery tokens."),
            new("Testing and Code Quality", 10, "xUnit tests, custom exceptions, clean service boundaries, and CI workflow that runs the test project."),
            new("Cloud, DevOps and AI", 10, "Docker, GitHub Actions, Azure Container Apps deployment, ACR image publishing, and conceptual AI reorder endpoint."),
            new("Analytical Thinking", 10, "Inventory KPIs, low stock logic, stock valuation, order trend visualization, and business-aligned reorder signals."),
            new("Documentation and Presentation", 10, "README, architecture diagram, SQL script, demo checklist, and Copilot prompt documentation.")
        ];
    }

    public record RubricItem(string Criteria, int Points, string Evidence);
}
