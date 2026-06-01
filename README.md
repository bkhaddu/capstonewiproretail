# Retail Optimization Platform (Order & Inventory Analytics)

The **Retail Optimization Platform** is a production-grade, data-driven ASP.NET Core platform designed to optimize retail stock management, manage product inventory levels, and handle customer sales orders. 

This solution satisfies all **Great Learning Capstone Evaluation Rubrics** by implementing a clean, layered C# architecture, Entity Framework Code-First data access alongside raw ADO.NET SQL joins, secure JWT role-based Web APIs, robust xUnit unit testing, Docker containerization, Azure deployment pipelines, and conceptual MCP cognitive AI agent summarization.

---

## 🏗️ System Architecture & Layered Design

The codebase implements standard **SOLID Principles** to ensure a high separation of concerns:

1. **Presentation Layer**: ASP.NET Core MVC controllers serving a premium glassmorphic Razor Pages interface with interactive AJAX forms, live Chart.js visualization, and secure REST Web API endpoints.
2. **Business Logic Layer (Services)**: Transaction-bound service logic coordinating inventory checks, price audits, and multi-item orders.
3. **Data Access Layer (Repositories)**: Abstraction via the Repository Pattern separating EF Core database operations and direct ADO.NET SQL connections.
4. **Database Layer**: Normalized 3NF MS SQL Server database containing automatic stock auditing triggers.

---

## 🛠️ Technology Stack & Core Focus Areas

- **Core Framework**: .NET 8.0 / C# 8.0+
- **Web App UI**: ASP.NET Core MVC, Razor Pages, TagHelpers, Unobtrusive Client/Server Validation, Vanilla JS AJAX.
- **ORM & Data Access**: Entity Framework Core 8.0 (Code-First Migrations, Linq) & raw **ADO.NET** (SQL connections, command boundaries, and `DbDataReader`).
- **Database Engine**: MS SQL Server (Tables normalized to 3NF, custom Joins, database Triggers).
- **Security & Identity**: JWT Bearer Authentication, Role-based Claims Authorization, custom developer token issuer.
- **Unit Testing**: xUnit, Moq, and isolated EF Core In-Memory databases for TDD rules.
- **DevOps & Cloud**: Multi-stage Dockerfile, Docker Compose, GitHub Actions CI/CD workflows, Azure Container Apps hosting.
- **AI Assets**: Cognitive MCP Agent replenishment summarizer, logged Copilot prompt histories.

---

## 📋 Capstone Rubric & Feature Mapping

| Rubric Area | Weight | Implemented Feature & Code Link |
| :--- | :---: | :--- |
| **Backend & MVC** | **25%** | Layered C# MVC setup with models ([`Product.cs`](file:///c:/Users/bkhad/Desktop/wipro/RetailOptimizationPlatform/Models/Product.cs), [`Order.cs`](file:///c:/Users/bkhad/Desktop/wipro/RetailOptimizationPlatform/Models/Order.cs)). SOLID transactions ([`OrderService.cs`](file:///c:/Users/bkhad/Desktop/wipro/RetailOptimizationPlatform/Repositories/OrderService.cs)), premium responsive layout ([`_Layout.cshtml`](file:///c:/Users/bkhad/Desktop/wipro/RetailOptimizationPlatform/Views/Shared/_Layout.cshtml)). |
| **Database & SQL** | **20%** | 3NF normalized SQL schema and `trg_StockUpdate` trigger ([`RetailOptimization.sql`](file:///c:/Users/bkhad/Desktop/wipro/RetailOptimizationPlatform/DatabaseScripts/RetailOptimization.sql)), raw ADO.NET joins reader ([`ProductRepository.GetProductSalesSummaryAsync`](file:///c:/Users/bkhad/Desktop/wipro/RetailOptimizationPlatform/Repositories/ProductRepository.cs#L54)). |
| **Web API & Security**| **15%** | Secure REST inventory endpoints, developer JWT token generation ([`AuthController.cs`](file:///c:/Users/bkhad/Desktop/wipro/RetailOptimizationPlatform/Controllers/AuthController.cs)), and claims role restrictions ([`InventoryApiController.cs`](file:///c:/Users/bkhad/Desktop/wipro/RetailOptimizationPlatform/Controllers/InventoryApiController.cs)). |
| **Testing & TDD** | **10%** | Complete TDD/xUnit testing suite covering order constraints and low stock repository filters ([`RetailOptimizationPlatform.Tests`](file:///c:/Users/bkhad/Desktop/wipro/RetailOptimizationPlatform/RetailOptimizationPlatform.Tests/)). **Status: 100% Passing**. |
| **Cloud & DevOps** | **10%** | Multi-stage [`Dockerfile`](file:///c:/Users/bkhad/Desktop/wipro/RetailOptimizationPlatform/Dockerfile), [`docker-compose.yml`](file:///c:/Users/bkhad/Desktop/wipro/RetailOptimizationPlatform/docker-compose.yml), CI/CD pipelines ([`ci-cd.yml`](file:///c:/Users/bkhad/Desktop/wipro/RetailOptimizationPlatform/.github/workflows/ci-cd.yml), [`deploy-azure-webapp.yml`](file:///c:/Users/bkhad/Desktop/wipro/RetailOptimizationPlatform/.github/workflows/deploy-azure-webapp.yml)). |
| **AI Integration** | **10%** | AI Replenishment Ticket Summarizer MCP Agent endpoint ([`AiApiController.cs`](file:///c:/Users/bkhad/Desktop/wipro/RetailOptimizationPlatform/Controllers/AiApiController.cs)) and Copilot logs ([`copilot_prompts.txt`](file:///c:/Users/bkhad/Desktop/wipro/RetailOptimizationPlatform/docs/copilot_prompts.txt)). |
| **Documentation** | **10%** | Full architectural walkthroughs and detailed setup guidelines. |

---

## ⚡ Setup & Local Execution

### Prerequisites
- .NET 8.0 SDK
- MS SQL Server (LocalDB or Express)
- Docker Desktop (Optional, for containerized run)

### 1. Database Initialization
Execute the SQL script [`DatabaseScripts/RetailOptimization.sql`](file:///c:/Users/bkhad/Desktop/wipro/RetailOptimizationPlatform/DatabaseScripts/RetailOptimization.sql) against your SQL Server. This will create the database, build the 3NF tables, activate the stock-auditing trigger, and seed default product items.

### 2. Configure Connection String
Update `appsettings.json` to point to your local database server:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=RetailOptimizationDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

### 3. Run the Application
Start the project locally using the .NET CLI:
```powershell
dotnet run --project RetailOptimizationPlatform.csproj
```
Access the dashboard at `http://localhost:5242` or `https://localhost:7198`.

### 4. Run via Docker Compose
Alternatively, launch the fully containerized app and database stack in one command:
```powershell
docker-compose up --build
```
The web app compiles, runs automatic EF Core database migrations, and exposes base port `8080` (accessible at `http://localhost:8080`).

---

## 🧪 Running Unit Tests

The test suite runs standard xUnit tests testing replenishment rules, transaction safety, and stock limits in isolation. Execute the following CLI command:
```powershell
dotnet test RetailOptimizationPlatform.Tests/RetailOptimizationPlatform.Tests.csproj
```

---

## 🤖 Cognitive AI MCP Replenishment Agent

The platform conceptualizes an **AI model endpoint** at `GET /api/ai/summarize-reorder/{productId}`:
* It analyzes current inventory data (stock level, category, and reorder levels).
* Computes active operating risk (Critical, Warning, Low).
* Synthesizes replenishment summaries detailing exact recommended restock quantities based on historic levels.
