# Library Management API

A clean architecture library management system with REST API and gRPC communication.

## 🚀 Quick Start

### Running the Application

#### 1. Quick Start - Run Api + gRPC server + tests
```bash
powershell -File run-tests.ps1
```

If above script fails try running the below script first and re-try (this will kill all processes with name "dotnet.exe")
```bash
Taskkill.exe /F /IM dotnet.exe /T
```

### Run All Tests
```bash
dotnet test Library.Tests/Library.Tests.csproj --verbosity normal
```

**Note:** E2E tests will fail with connection errors if services aren't running. This is expected behavior.

## 📁 Project Structure

```
LibrarySystem/
├── Library.Api/                    # REST API Layer
├── Libary.Backend.Grpc/           # gRPC Service Layer
├── Library.Backend.Application/   # Business Logic
├── Library.Backend.Infrastructure/ # Data Access
├── Library.Backend.Domain/        # Domain Entities
├── Library.Shared.Contracts/      # gRPC Contracts
├── Library.Tests/                 # All Tests
└── Starters/                      # Starters - Warm-up Tasks
```

## 📊 Technologies

- .NET 10
- ASP.NET Core
- gRPC
- Entity Framework Core
- SQLite
- xUnit


## 👤 Author

Vihanga Bandara
