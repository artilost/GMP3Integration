# GMP3Integration

## 📌 About the Project
GMP3Integration is a .NET 8 based layered architecture project designed to integrate seamlessly with Ingenico GMP3 devices.  
The project includes API endpoints covering all FP3_* functions, enabling operations such as payment processing, receipt printing, department configuration, and tax rate management.

---

## 🏗 Architecture
The project follows a **layered architecture** approach:

- **Domain** → Core business rules, entities, and models  
- **Application** → Service interfaces, DTOs, workflow management  
- **Infrastructure** → Implementations for communication with the GMP3 device (FP3_* stub methods)  
- **API** → Controllers, middleware, endpoint definitions  

---

## ⚙️ Features
- **FP3_* Stub Methods**  
  - `FP3_Start`, `FP3_OptionFlags`, `FP3_TicketHeader`, `FP3_ItemSale`, `FP3_Payment`,  
    `FP3_PrintTotalsAndPayments`, `FP3_PrintBeforeMF`, `FP3_PrintMF`, `FP3_Close`
- **CompleteSale Workflow** → Single endpoint to execute the entire sale process end-to-end  
- **Department & Tax Configuration** endpoints  
- **Transaction Scope** and **Correlation ID** middleware  
- **Error Handling**: `ApiExceptionMiddleware`  
- **Logging**: Logs the entire transaction flow  
- **FluentValidation** integration  
- **Swagger** documentation  

---

## 📡 API Endpoints
| HTTP Method | Endpoint | Description |
|-------------|----------|-------------|
| `POST` | `/api/gmp3/complete-sale` | Completes the sale from start to finish |
| `POST` | `/api/gmp3/start` | Starts a transaction |
| `POST` | `/api/gmp3/option-flags` | Sets option flags |
| `POST` | `/api/gmp3/ticket-header` | Sets ticket header |
| `POST` | `/api/gmp3/item-sale` | Adds an item sale |
| `POST` | `/api/gmp3/payment` | Adds a payment transaction |
| `POST` | `/api/gmp3/print-totals-and-payments` | Prints totals and payment details |
| `POST` | `/api/gmp3/print-before-mf` | Prints before fiscal receipt |
| `POST` | `/api/gmp3/print-mf` | Prints fiscal receipt |
| `POST` | `/api/gmp3/close` | Closes the transaction |
| `GET`  | `/api/gmp3/tax-rates` | Lists tax rates |
| `POST` | `/api/gmp3/set-departments` | Configures departments |

---

## 🚀 Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/USERNAME/GMP3Integration.git
