# DVLD-System

Driving & Vehicle License Department System built with C#, WinForms, SQL Server, ADO.NET, and layered architecture.

---

# Overview

DVLD-System is a desktop application designed to manage driving licenses, drivers, applications, tests, users, and permissions using a clean 3-tier architecture approach.

The project focuses on:

* Database design
* Business logic separation
* Authentication system implementation
* Real-world workflow simulation
* Backend architecture concepts

---

# Features

* User Authentication System
* Drivers & People Management
* License Applications
* Test Appointments & Tests
* License Management
* Roles & Permissions System
* Search & Filtering
* Async/Await Operations
* Layered Architecture (Presentation, Business, Data Access)

---

# Architecture

The project follows a 3-Tier Architecture:

```text
UI Layer (WinForms)
        ↓
Business Layer (BL)
        ↓
Data Access Layer (DAL)
        ↓
SQL Server Database
```

---

# Technologies Used

* C#
* WinForms
* SQL Server
* ADO.NET
* Async/Await
* LINQ
* DTO Pattern
* Layered Architecture

---

# Database Setup

## 1. Open SQL Server Management Studio (SSMS)

## 2. Run the database script

Open:

```text
Database/DVLD.sql
```

Execute the script to create:

* Database
* Tables
* Relationships
* Seed data

---

## 3. Configure Connection String

Update the connection string if needed:

```csharp
Server=.;Database=MeDVLD;Integrated Security=True;
```

---

## 4. Build and Run

Open:

```text
DVLD.sln
```

Then:

* Build Solution
* Run the application

---

# Screenshots

The current focus of this project is backend architecture, database design, and business logic implementation.

UI improvements and redesigns will be added in future updates.

---

# Project Goals

This project was built to practice and improve:

* Database design
* Backend architecture
* Clean code principles
* Real-world business workflows
* Desktop application development

---

# Future Improvements

* ASP.NET Core Web API version
* RESTful API implementation
* JWT Authentication
* Entity Framework Core
* Reporting System
* Dashboard & Analytics
* Better UI/UX
* Logging & Auditing

---

# YouTube Content

Parts of this project are being documented and explained on YouTube, focusing on backend architecture, database design, layered architecture, and implementation details.

YouTube Channel:
https://www.youtube.com/@BackendMindset

# Author

BackendMindset
