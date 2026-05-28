# DVLD-System

Driving & Vehicle License Department System built with C#, WinForms, SQL Server, ADO.NET, and layered architecture.

---

# Overview

DVLD-System is a desktop application designed to manage driving licenses, drivers, applications, tests, users, and permissions using a clean 3-tier architecture approach.

The project focuses on:
- Database design
- Business logic separation
- Authentication & authorization concepts
- Real-world workflow simulation

---

# Features

- User Authentication System
- Drivers & People Management
- License Applications
- Test Appointments & Tests
- License Management
- Roles & Permissions System
- Search & Filtering
- Async/Await Operations
- Layered Architecture (Presentation, Business, Data Access)

---

# Architecture

The project follows a 3-Tier Architecture:

UI Layer (WinForms)
↓
Business Layer (BL)
↓
Data Access Layer (DAL)
↓
SQL Server Database

---

# Technologies Used

- C#
- WinForms
- SQL Server
- ADO.NET
- Async/Await
- LINQ
- DTO Pattern
- Layered Architecture

---

# Database Setup

## 1. Open SQL Server Management Studio (SSMS)

## 2. Run the database script

Open:

Database/DVLD.sql

Execute the script to create:
- Database
- Tables
- Relationships
- Seed data

---

## 3. Configure Connection String

Update the connection string if needed:

```csharp
Server=.;Database=MeDVLD;Integrated Security=True;
