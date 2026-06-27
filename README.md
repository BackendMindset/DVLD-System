# DVLD - Driver & Vehicle Licensing Department

A professional **Desktop Management System** built with **C#, WinForms, ADO.NET, and SQL Server**, following a clean **3-Layer Architecture** to simulate the workflow of a real governmental Driver & Vehicle Licensing Department.

The system manages the complete lifecycle of driving license services, including people registration, user management, driver records, license applications, testing procedures, license issuance, payments, and traffic violations.

Rather than being a simple CRUD application, DVLD focuses on implementing real business workflows and validation rules. Every operation passes through the Business Layer, where business decisions are enforced before interacting with the database. This approach improves maintainability, scalability, and code quality while keeping responsibilities clearly separated across the application.

The project demonstrates practical software engineering concepts including layered architecture, DTO mapping, asynchronous programming, centralized validation, reusable components, structured error handling, and clean code organization.

---

# Project Highlights

The project was designed with maintainability and scalability in mind. Instead of placing business logic inside the user interface or database layer, all business rules are centralized within the Business Layer.

Key features include:

* Clean 3-Layer Architecture
* SQL Server Relational Database
* ADO.NET Data Access
* Async/Await Programming
* DTO Pattern
* Result Pattern
* Business Validation
* Thread-Safe Logging System
* Search & Filtering
* Role-Based User Management
* Modular WinForms Interface
* Reusable Components
* Separation of Concerns

---

# Architecture

The application follows a classic 3-Layer Architecture to separate responsibilities and simplify future maintenance.

```text
                Presentation Layer (WinForms)
                         │
                         ▼
                 Business Layer (BL)
        Business Rules • Validation • DTOs
                         │
                         ▼
              Data Access Layer (DAL)
          SQL Queries • CRUD • Data Mapping
                         │
                         ▼
                    SQL Server Database
```

### Presentation Layer

The Presentation Layer provides the Windows Forms user interface. It is responsible for displaying information, collecting user input, and communicating with the Business Layer without containing business logic.

### Business Layer

The Business Layer acts as the core of the application. It contains all business rules, validation logic, workflow management, DTO mapping, and decision-making processes. Every operation must pass through this layer before reaching the database.

### Data Access Layer

The Data Access Layer communicates directly with SQL Server using ADO.NET. It performs CRUD operations, executes SQL queries, maps database records to objects, and isolates all database-related code from the rest of the application.

---

# Technologies

| Category        | Technology           |
| --------------- | -------------------- |
| Language        | C#                   |
| Framework       | .NET Framework 4.7.2 |
| Desktop UI      | Windows Forms        |
| Database        | SQL Server           |
| Data Access     | ADO.NET              |
| IDE             | Visual Studio        |
| Version Control | Git & GitHub         |

---

# Project Structure

```text
DVLD
│
├── DVLD
│   ├── Applications
│   ├── Drivers
│   ├── Licenses
│   ├── Login
│   ├── Main
│   ├── MedicalCenters
│   ├── Payments
│   ├── People
│   ├── Roles
│   ├── Shared
│   ├── Tests
│   ├── Users
│   └── Violations
│
├── DVLD.BusinessLayer
│   ├── Entities
│   ├── DTOs
│   ├── Services
│   ├── Mappers
│   ├── Logger
│   └── Result Pattern
│
└── DVLD.DataAccessLayer
    ├── SQL Operations
    ├── Data Mapping
    └── Database Configuration
```

---

# Main Modules

## People Management

Manages all personal information used throughout the system.

Features:

* Register new people
* Edit personal information
* Delete records
* Search by multiple criteria
* Input validation
* Duplicate detection

---

## User Management

Responsible for authentication-related user accounts and system access.

Features:

* Create new users
* Activate or deactivate accounts
* Change passwords
* Search users
* Manage account status

---

## Driver Management

Maintains driver information and driving history.

Features:

* Driver registration
* Driver profile management
* License history
* Driver search

---

## Application Management

Handles the complete driving license application workflow.

Features:

* Create applications
* Track application progress
* Update application status
* Cancel completed or pending applications
* Business workflow validation

---

## License Management

Responsible for issuing and managing driving licenses.

Features:

* Local licenses
* International licenses
* License suspension
* License restoration
* Detained licenses
* License history

---

## Test Management

Controls testing procedures before issuing licenses.

Features:

* Schedule appointments
* Medical tests
* Written tests
* Practical tests
* Test result management

---

## Payment Management

Records all financial transactions related to licensing services.

Features:

* Register payments
* Payment tracking
* Payment history
* Payment details

---

## Violation Management

Handles traffic violations associated with licenses.

Features:

* Record violations
* Search violations
* View violation details
* Violation history

---

# Business Layer

The Business Layer is the heart of the application.

Instead of allowing the user interface to communicate directly with the database, every operation passes through the Business Layer where business rules are validated before execution.

Its responsibilities include:

* Business Validation
* Workflow Management
* Decision Making
* DTO Mapping
* Service Coordination
* Result Handling
* Logging Integration

### Business Operations

* ActivateUserAsync()
* DeactivateUserAsync()
* CancelApplicationAsync()
* CompleteApplicationAsync()
* SuspendLicenseAsync()
* RestoreLicenseAsync()

### Validation

* ValidatePersonAsync()
* ValidateUserAsync()
* ValidateDriverAsync()
* ValidateLicenseAsync()

### Business Decisions

* CanCreateUserAsync()
* CanDeletePersonAsync()
* CanIssueLicenseAsync()
* CanIssueInternationalLicenseAsync()
* CanDetainLicenseAsync()
* CanReleaseDetainedLicenseAsync()
* CanScheduleTestAsync()

### Search Operations

* SearchPersonsAsync()
* SearchUsersAsync()
* SearchDriversAsync()
* SearchApplicationsAsync()
* SearchLicensesAsync()
* SearchViolationsAsync()

Examples of implemented business rules include:

* A user cannot exist without an associated person.
* Duplicate applications are prevented.
* A license cannot be issued before passing the required tests.
* International licenses are only available for eligible drivers.
* Suspended licenses cannot be renewed until restored.

---

# Design Patterns & Principles

The project applies several software engineering principles to improve maintainability and code quality.

Implemented concepts include:

* 3-Layer Architecture
* DTO Pattern
* Result Pattern
* Separation of Concerns
* Single Responsibility Principle (SRP)
* Async Programming
* Reusable Components
* Mapping Layer
* Business Validation

---

# Logging & Error Handling

The application includes a centralized logging and error handling mechanism to improve reliability and simplify debugging.

Features include:

* Global Exception Handling
* Thread-Safe Logger
* Standardized Result<T> Responses
* User-Friendly Error Messages
* Safe Database Operations
* Validation Error Reporting

---

# Database

The system uses Microsoft SQL Server with a normalized relational database designed to model the complete licensing workflow.

The database includes tables for:

* People
* Users
* Drivers
* Applications
* License Classes
* Licenses
* Local Driving License Applications
* International Licenses
* Tests
* Test Appointments
* Payments
* Violations
* Medical Centers
* Roles
* Permissions

Relationships are maintained using primary keys, foreign keys, constraints, and indexes to ensure data integrity and efficient querying.

---

# System Workflow

A typical request inside the application follows this sequence:

1. The user performs an action through the WinForms interface.
2. The Presentation Layer sends the request to the Business Layer.
3. The Business Layer validates business rules.
4. If validation succeeds, the request is forwarded to the Data Access Layer.
5. The Data Access Layer executes SQL operations against SQL Server.
6. Results are returned to the Business Layer.
7. The Presentation Layer updates the user interface.
8. Errors and important events are recorded by the logging system.

---

# Current Status

Current implementation includes:

* Database completed
* Data Access Layer completed
* Business Layer completed
* Core application logic completed
* Modular WinForms UI under development

---

# Future Roadmap

Planned improvements include:

* Complete remaining WinForms modules
* RESTful Web API
* ASP.NET Core Migration
* Web Frontend
* JWT Authentication
* Entity Framework Core Version
* Unit Testing
* Dependency Injection
* Reporting & Dashboard Enhancements

---

# Why This Project?

DVLD was developed as a comprehensive portfolio project to simulate the architecture and workflow of a real governmental licensing system.

The project demonstrates practical experience with:

* Desktop Application Development
* Backend-Oriented System Design
* SQL Server Database Design
* Business Rule Implementation
* Layered Architecture
* Asynchronous Programming
* Clean Code Organization
* Maintainable Software Architecture

Rather than focusing solely on CRUD operations, the project emphasizes real-world workflows, business validation, and scalable software design principles commonly used in enterprise applications.

---

# Author

**Eslam Nasr**

Backend Developer specializing in **C#, .NET, SQL Server, WinForms, and Backend Architecture**.

## GitHub

https://github.com/BackendMindset
