# GMP3 Integration Project

GMP3 Fiscal Device Integration with .NET 8.0 and Clean Architecture

## 🚀 Features

- **Clean Architecture** with 4 layers (Domain, Application, Infrastructure, API)
- **CQRS Pattern** with MediatR
- **P/Invoke Integration** with GMPSmartDLL
- **Session Management** for device communication
- **Resilience Patterns** with Polly (retry, circuit breaker, timeout)
- **Structured Logging** with Serilog
- **Rate Limiting** for API protection
- **JSON Serialization** for native DLL communication

## ⚠️ Known Issues

- **Payment functionality** returns `0xF025` (JSON_INVALID_INTERFACE) error
- JSON serialization format needs debugging
- Emulator compatibility issues

## 🛠️ Technologies Used

- .NET 8.0
- ASP.NET Core
- MediatR
- Serilog
- FluentValidation
- Polly
- System.Text.Json
- Microsoft.AspNetCore.RateLimiting

## 📁 Project Structure

```
GMP3Integration/
├── GMP3Integration.API/          # Web API layer
├── GMP3Integration.Application/  # Use cases and DTOs
├── GMP3Integration.Domain/       # Business entities
├── GMP3Integration.Infrastructure/ # External dependencies
└── README.md
```

## 🚀 Getting Started

1. Clone the repository
2. Restore NuGet packages: `dotnet restore`
3. Build the solution: `dotnet build`
4. Run the API: `dotnet run --project GMP3Integration.API`

## 📋 API Endpoints

- `POST /api/gmp3/start` - Start new transaction
- `POST /api/gmp3/complete-sale` - Complete sale workflow

## 🔧 Development Status

- ✅ Project setup and architecture
- ✅ Native DLL integration
- ✅ Session management
- ✅ API controllers
- ⚠️ Payment processing (has issues)
- ✅ Error handling and logging
- ✅ Resilience patterns

## 📚 Documentation

See `PROJE_DEVREDILME_DOKUMANI.md` for detailed code examples and explanations.

## 👨‍💻 Author

Mustafa Kalender - Internship Project 2025
