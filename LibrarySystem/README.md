# Library Management API

A clean architecture library management system with REST API and gRPC communication.

## 🚀 Quick Start

### Running the Application

#### 1. Start the gRPC Backend Server
```bash
cd Libary.Backend.Grpc
dotnet run
```
The gRPC server will start on `http://localhost:5000`

#### 2. Start the REST API (in a new terminal)
```bash
cd Library.Api
dotnet run
```
The API will start on `http://localhost:5001`

### Testing the API

Once both servers are running, you can run the tests

**API Documentation:**
Navigate to `http://localhost:5001/scalar/v1` in your browser for interactive API documentation.

---

## 🧪 Running Tests

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
└── Library.Tests/                 # All Tests
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
