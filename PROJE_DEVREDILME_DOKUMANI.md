# GMP3 Integration Projesi - Devir Dokümanı

## 📋 Proje Özeti

**Proje Adı:** GMP3 Integration  
**Teknoloji:** .NET 8.0, ASP.NET Core, Clean Architecture  
**Amaç:** GMP3 Fiskal Cihazı ile entegrasyon  
**Durum:** Payment özelliği WIP (Work In Progress)  
**Geliştirici:** Mustafa Kalender  
**Tarih:** 2025  

## 🎯 Proje Amacı

Bu proje, GMP3 fiskal cihazı ile .NET uygulamaları arasında entegrasyon sağlamak için geliştirilmiştir. Clean Architecture prensiplerine uygun olarak tasarlanmış ve modern .NET teknolojileri kullanılarak implement edilmiştir.

## 🏗️ Mimari Yapı

### Clean Architecture Katmanları:

```
GMP3Integration/
├── GMP3Integration.Domain/          # İş mantığı ve varlıklar
├── GMP3Integration.Application/     # Use case'ler ve DTO'lar
├── GMP3Integration.Infrastructure/  # Dış bağımlılıklar ve servisler
└── GMP3Integration.API/             # Web API controller'ları
```

### Katman Sorumlulukları:

- **Domain:** Business entities, enums, domain logic
- **Application:** Use cases, DTOs, interfaces, CQRS handlers
- **Infrastructure:** P/Invoke, native DLL integration, external services
- **API:** Controllers, middleware, configuration

## 🛠️ Kullanılan Teknolojiler

### Core Technologies:
- **.NET 8.0** - Modern C# framework
- **ASP.NET Core** - Web API framework
- **C# 12** - Programming language

### Design Patterns & Libraries:
- **MediatR** - CQRS pattern implementation
- **Serilog** - Structured logging
- **FluentValidation** - Input validation
- **Polly** - Resilience patterns (retry, circuit breaker, timeout)
- **System.Text.Json** - JSON serialization
- **Microsoft.AspNetCore.RateLimiting** - API protection

### Native Integration:
- **P/Invoke** - Native DLL integration
- **GMPSmartDLL.dll** - GMP3 device communication

## 📁 Proje Yapısı Detayı

### Domain Layer (`GMP3Integration.Domain/`)
```
Domain/
├── Enums/
│   ├── TTicketType.cs          # Ticket type enum'ları
│   └── PaymentTypes.cs         # Payment type enum'ları
└── Entities/
    └── (Domain entities)
```

### Application Layer (`GMP3Integration.Application/`)
```
Application/
├── DTOs/                       # Data Transfer Objects
│   ├── Payment/
│   │   ├── PaymentRequest.cs   # Payment request DTO
│   │   └── PaymentResponse.cs  # Payment response DTO
│   ├── TicketHeader/
│   ├── ItemSale/
│   └── ...
├── Features/                   # CQRS Commands/Queries
│   ├── Commands/
│   │   ├── CompleteSale/
│   │   └── StartTransaction/
│   └── Queries/
├── Interfaces/                 # Service interfaces
├── Services/                   # Application services
└── Validators/                 # FluentValidation validators
```

### Infrastructure Layer (`GMP3Integration.Infrastructure/`)
```
Infrastructure/
├── Interop/                    # P/Invoke integration
│   ├── Gmp3NativeMethods.cs   # Native method wrappers
│   ├── Native/
│   │   ├── Structs/           # Native struct definitions
│   │   ├── Enums/             # Native enum definitions
│   │   └── PInvoke/           # P/Invoke declarations
│   └── Constants/             # Native constants
├── Services/                   # Infrastructure services
│   ├── Gmp3InteropService.cs  # Core service implementation
│   ├── Decorators/            # Service decorators
│   └── Pairing/               # Device pairing services
├── Session/                    # Session management
│   └── Gmp3SessionManager.cs  # Session state management
└── Configuration/              # Configuration classes
```

### API Layer (`GMP3Integration.API/`)
```
API/
├── Controllers/
│   └── Gmp3Controller.cs      # Main API controller
├── Filters/                    # Action filters
├── Middlewares/                # Custom middlewares
├── Program.cs                  # Application startup
└── appsettings.json           # Configuration
```

## 🔧 Temel Bileşenler

### 1. Session Management
**Dosya:** `GMP3Integration.Infrastructure/Session/Gmp3SessionManager.cs`

```csharp
public static class Gmp3SessionManager
{
    public static uint InterfaceHandle { get; set; }
    public static ulong TransactionHandle { get; set; }
    public static string Interface { get; set; }
    
    public static bool IsSessionActive => InterfaceHandle > 0 && !string.IsNullOrEmpty(Interface);
    public static bool IsTransactionActive => TransactionHandle > 0;
}
```

**Amaç:** GMP3 cihazı ile olan bağlantının durumunu ve handle'ları yönetir.

### 2. Native DLL Integration
**Dosya:** `GMP3Integration.Infrastructure/Interop/Gmp3NativeMethods.cs`

```csharp
[DllImport("GMPSmartDLL.dll", EntryPoint = "Json_FP3_Payment", 
           CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
public static extern uint Json_FP3_Payment(uint hInt, ulong hTran, byte[] szJsonData, 
                                          int JsonDataLen, byte[] szResponse, 
                                          int ResponseLen, int TimeoutInMiliseconds);
```

**Amaç:** Native DLL method'larını C# tarafında kullanılabilir hale getirir.

### 3. Payment Processing
**Dosya:** `GMP3Integration.Infrastructure/Services/Gmp3InteropService.cs`

```csharp
public async Task<PaymentResponse> MakePaymentAsync(PaymentRequest request)
{
    // Session validation
    // DTO to native struct conversion
    // Native DLL call
    // Error handling
}
```

**Amaç:** Ödeme işlemlerini yönetir.

### 4. CQRS Pattern
**Dosya:** `GMP3Integration.Application/Features/Commands/CompleteSale/CompleteSaleHandler.cs`

```csharp
public class CompleteSaleHandler : IRequestHandler<CompleteSaleCommand, CompleteSaleResponse>
{
    public async Task<CompleteSaleResponse> Handle(CompleteSaleCommand command, CancellationToken cancellationToken)
    {
        // Business logic implementation
    }
}
```

**Amaç:** Command/Query ayrımı ile modüler yapı sağlar.

## 🚨 Mevcut Durum ve Sorunlar

### ✅ Tamamlanan Özellikler:
- [x] Clean Architecture kurulumu
- [x] CQRS pattern implementasyonu
- [x] P/Invoke native DLL entegrasyonu
- [x] Session management
- [x] API controller'ları
- [x] Error handling ve logging
- [x] Resilience patterns (Polly)
- [x] Rate limiting
- [x] Structured logging (Serilog)

### ⚠️ Work In Progress:
- [ ] **Payment Processing** - Ana sorun burada!

### 🚨 Bilinen Sorunlar:

#### 1. Payment 0xF025 Hatası
**Hata Kodu:** `0xF025` (JSON_INVALID_INTERFACE)  
**Dosya:** `GMP3Integration.Infrastructure/Interop/Gmp3NativeMethods.cs`  
**Satır:** JSON serialization kısmı  

**Sorun:** Native DLL, gönderilen JSON formatını kabul etmiyor.

**Mevcut JSON Format:**
```json
{
  "typeOfPayment": 1,
  "subtypeOfPayment": 0,
  "payAmount": 1000,
  "payAmountCurrencyCode": 949,
  "bankBkmId": 0,
  "BankPaymentUniqueId": "test-payment-123",
  "payAmountBonus": 0,
  "numberOfinstallments": 0,
  "transactionFlag": 0
}
```

**Çözüm Önerileri:**
1. Emulator loglarını incele
2. JSON field isimlerini kontrol et
3. Data type'ları doğrula
4. Native struct ile JSON arasındaki uyumsuzluğu bul

#### 2. Session Handle Yönetimi
**Dosya:** `GMP3Integration.Infrastructure/Session/Gmp3SessionManager.cs`  
**Sorun:** Static session management, multi-user senaryolarda sorun yaratabilir.

## 🔍 Debug ve Test

### Test Endpoint'leri:
```bash
# Transaction başlat
POST /api/gmp3/start
{
  "currentInterface": "TCP:192.168.137.99:7500"
}

# Complete sale (payment dahil)
POST /api/gmp3/complete-sale
{
  "items": [...],
  "payments": [...]
}
```

### Log Dosyaları:
- Serilog ile structured logging yapılıyor
- Log level: Information, Warning, Error
- Console ve file output

### Debug Adımları:
1. **Payment JSON Debug:**
   ```csharp
   // Gmp3NativeMethods.cs içinde
   var paymentJson = System.Text.Json.JsonSerializer.Serialize(new { ... });
   _log.LogInformation("Payment JSON: {json}", paymentJson);
   ```

2. **Session State Debug:**
   ```csharp
   _log.LogInformation("Session Info: {info}", Gmp3SessionManager.GetSessionInfo());
   ```

3. **Native Method Debug:**
   ```csharp
   _log.LogInformation("Native method result: 0x{rc:X}", result);
   ```

## 📚 Önemli Dosyalar

### 1. Ana Konfigürasyon
- `GMP3Integration.API/Program.cs` - Dependency injection, middleware
- `GMP3Integration.API/appsettings.json` - Configuration

### 2. Core Services
- `GMP3Integration.Infrastructure/Services/Gmp3InteropService.cs` - Ana service
- `GMP3Integration.Infrastructure/Session/Gmp3SessionManager.cs` - Session yönetimi

### 3. Native Integration
- `GMP3Integration.Infrastructure/Interop/Gmp3NativeMethods.cs` - P/Invoke wrappers
- `GMP3Integration.Infrastructure/Interop/Native/Structs/Gmp3Structs.cs` - Native structs

### 4. API Layer
- `GMP3Integration.API/Controllers/Gmp3Controller.cs` - REST endpoints

### 5. Application Layer
- `GMP3Integration.Application/Services/TransactionWorkflowService.cs` - Workflow orchestration
- `GMP3Integration.Application/Features/Commands/` - CQRS commands

## 🚀 Geliştirme Ortamı Kurulumu

### Gereksinimler:
- .NET 8.0 SDK
- Visual Studio 2022 veya VS Code
- GMP3 Emulator (test için)
- GMPSmartDLL.dll (native library)

### Kurulum Adımları:
1. Repository'yi clone et
2. `dotnet restore` çalıştır
3. `dotnet build` ile build et
4. `dotnet run --project GMP3Integration.API` ile çalıştır

### Test:
- Swagger UI: `https://localhost:7000/swagger`
- API endpoints test et

## 🔧 Sonraki Adımlar

### 1. Payment Sorunu Çözümü (Öncelik 1):
- [ ] Emulator loglarını incele
- [ ] JSON formatını debug et
- [ ] Native struct ile JSON uyumluluğunu kontrol et
- [ ] Test case'leri yaz

### 2. İyileştirmeler:
- [ ] Multi-user session management
- [ ] Unit test'ler ekle
- [ ] Integration test'ler yaz
- [ ] Performance optimization

### 3. Dokümantasyon:
- [ ] API dokümantasyonu tamamla
- [ ] Code comments ekle
- [ ] Architecture decision records (ADR)

## 📞 İletişim

**Geliştirici:** Mustafa Kalender  
**Proje:** GMP3 Integration  
**Durum:** Payment WIP  

## 📝 Notlar

- Proje Clean Architecture prensiplerine uygun geliştirilmiştir
- Tüm external dependencies Infrastructure katmanında
- Business logic Application katmanında
- API sadece HTTP handling yapar
- Session management static olarak implement edilmiş (multi-user için iyileştirilebilir)
- Payment özelliği en kritik sorun, öncelikle bu çözülmeli

---

**Son Güncelleme:** 2025  
**Versiyon:** 1.0.0-WIP  
**Durum:** Development
