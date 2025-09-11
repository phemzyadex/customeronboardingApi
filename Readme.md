# 🏦 Customer Onboarding Microservice API

A clean-architecture based **ASP.NET Core Web API** for customer onboarding.  
It demonstrates **best practices** such as Domain-Driven Design, Entity Framework Core, JWT authentication, dependency injection, validation, and Swagger documentation.

---

## Features
- **Customer Onboarding**
  - Register with Email, Phone Number, Password, State, and LGA.
  - Phone number verification via **mocked OTP service**.
  - Prevents duplicate phone/email registration.
-  **State and LGA Management**
  - States and LGAs mapped with proper relationships.
  - Validation ensures LGA belongs to selected State.
-  **Authentication & Authorization**
  - Secure login with hashed passwords (BCrypt).
  - JWT tokens including `username`, `role`, and `expiry`.
-  **Bank Lookup**
  - Consumes external API [`GetBanks`](https://wema-alatdev-apimgt.developer.azure-api.net/api-details#api=alat-tech-test-api).
  - Returns a list of supported banks.
-  **Soft Delete**
  - Customers can be soft-deleted (marked as deleted without physical removal).
-  **Swagger UI**
  - Auto-generated documentation and interactive testing.
-  **Unit Testing**
  - Services are interface-driven, supporting mocking and isolated testing.

---

## 📂 Project Structure
```bash
CustomerOnboarding/
├── CustomerOnboarding.Api/           # Presentation layer (Controllers, Swagger, Middleware)
├── CustomerOnboarding.Core/          # Domain & Application layer (DTOs, Interfaces, Models, Validators)
├── CustomerOnboarding.Infrastructure # Infrastructure (EF Core, Services)
└── CustomerOnboarding.Tests/         # Unit tests

Tech Stack

.NET 9 (ASP.NET Core Web API)

Entity Framework Core
 + SQL Server

JWT Authentication
BCrypt.Net  for password hashing

FluentValidation for input validation

Swagger / Swashbuckle for API docs

xUnit for testing

HttpClientFactory for external API calls

Setup & Run 
1. Clone Repository
git clone https://github.com/phemzyadex/CustomerOnboarding.git
cd CustomerOnboarding

2. Configure Database

  Update database connection
   Edit `appsettings.json`:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=.;Database=ProductOrderDB;User Id=sa;Password=*****;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;Pooling=True;Max Pool Size=100;""
   }
   ```
3. Run Migrations
dotnet ef database update --project CustomerOnboarding.Infrastructure --startup-project CustomerOnboarding.Api

4. Run the API
dotnet run --project CustomerOnboarding.Api

5. Access Swagger UI

Navigate to:

https://localhost:7240/swagger

## Authentication

  Login
  ```json
  {
    "username": "admin",
    "password": "admin123"
  }
  ```


## API Endpoints (Summary)
  - Auth

    + POST /api/auth/register → Register new user
    + POST /api/auth/login → Login user (returns JWT)
    + GET /api/auth/users → List all users (Admin only)

  -  Customers
    + POST /api/customers/onboard → Onboard customer (sends OTP)
    + POST /api/customers/verify-phone → Verify OTP and complete onboarding
    + GET /api/customers → List all customers
    + DELETE /api/customers/{id} → Soft delete a customer

  - States 
    + POST /api/states → Add a new State
    + GET /api/states → Get all states

  - LGAs
    + POST /api/lgas → Add a new LGA
    + GET /api/states/{id}/lgas → Get LGAs by state

  - Banks
    = GET /api/banks → Fetch all banks (via external Wema API)

  - Testing

Run all tests:
dotnet test

---

Prepare by
Adeola Oluwafemi