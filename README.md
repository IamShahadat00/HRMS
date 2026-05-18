# HRMS Pro - Enterprise Human Resource Management System

HRMS Pro is a professional-grade, web-based application designed to automate organization staff management, financial pay structures, and monthly payroll processing. This project follows a **Decoupled N-Tier Architecture**, ensuring high scalability, security, and maintainability.

## 🚀 Key Features

- **Role-Based Access Control (RBAC):** Secure authentication system utilizing **JWT (JSON Web Tokens)** and Front-end AuthGuards.
- **Employee Lifecycle Management:** Full CRUD operations with server-side searching/filtering by name, email, or department.
- **Dynamic Salary Configuration:** One-to-One relational mapping to manage base pay, medical/conveyance allowances, and monthly deductions.
- **Automated Payroll Engine:** A custom business logic engine that generates monthly records, calculates **Progressive Income Tax**, and derives final Net Salary.
- **Modern UI/UX:** Responsive interface built with **Tailwind CSS 4.0** and interactive feedback via **SweetAlert2**.

## 🛠️ Technical Stack

### Backend
- **Framework:** ASP.NET Core 8.0/9.0 Web API
- **Language:** C#
- **Database:** Microsoft SQL Server
- **ORM:** Entity Framework Core (Code-First)
- **Security:** JWT Authentication & BCrypt.Net Password Hashing
- **JSON Handling:** NewtonsoftJson (Circular Reference Management)

### Frontend
- **Framework:** Angular 19 (Standalone Component Architecture)
- **Styling:** Tailwind CSS & Bootstrap Icons
- **State Management:** Reactive Forms & RxJS Observables
- **Networking:** HTTP Interceptors for global Bearer Token injection

## 🏗️ Architecture & Highlights

- **Clean Architecture:** Separated into 4 layers: `API`, `Core` (Entities/DTOs), `Data` (DbContext), and `Services` (Business Logic).
- **Data Integrity:** Implemented Eager Loading (`.Include()`) and DTO mapping to ensure optimized data flow and security.
- **Defensive Programming:** Implemented try-catch blocks and null-safe rendering to prevent application crashes during complex payroll runs.

## ⚙️ Setup Instructions

### Prerequisites
- .NET 8/9 SDK
- Node.js (LTS version)
- SQL Server

### Backend Setup
1. Update `appsettings.json` with your SQL Connection String.
2. Run `Update-Database` in the Package Manager Console.
3. Press `F5` to start the API (Swagger will open at `localhost`).

### Frontend Setup
1. Navigate to the `HRMS_Frontend/hrms` folder.
2. Run `npm install --legacy-peer-deps`.
3. Run `ng serve`.
4. Login with default credentials: `admin` / `Admin@123`.

---
*Developed by [Your Name] as a Full-Stack Job Preparation Project.*
