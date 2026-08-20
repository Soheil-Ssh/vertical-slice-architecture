# ASP.NET Core Vertical Slice Architecture

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet\&logoColor=white)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-Web_API-512BD4?logo=dotnet)](https://dotnet.microsoft.com/apps/aspnet)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Architecture](https://img.shields.io/badge/Architecture-Vertical_Slice-blue)](#architecture)

A practical reference implementation demonstrating **Vertical Slice Architecture** in ASP.NET Core.

The goal of this repository is to show how an application can be organized around business features instead of technical layers.

Unlike traditional layered architectures where code is grouped by type (Controllers, Services, Repositories), Vertical Slice Architecture organizes code by the capabilities and use cases of the system.

This project uses a simple Task Management domain to demonstrate how each feature can contain everything required to execute a business operation.

---

# Why This Project Exists

As applications grow, traditional layered architectures often create problems:

* Related code becomes spread across multiple folders
* Changing a feature requires navigating through many layers
* Business workflows become harder to understand

Vertical Slice Architecture approaches organization differently.

Instead of asking:

> "Which layer does this code belong to?"

it asks:

> "Which feature does this code belong to?"

Each feature becomes an independent slice containing its own request handling, validation, business flow, and endpoint definition.

---

# What You Will Learn

By exploring this repository, you will learn:

* How to structure ASP.NET Core applications using Vertical Slice Architecture
* How to organize code around features instead of technical concerns
* How Commands and Queries represent application use cases
* How MediatR can be used to handle feature execution
* How validation can be placed close to the related feature
* How Minimal APIs and Carter can be used for feature-based endpoints
* How this approach improves maintainability as applications grow

---

# Architecture Overview

The main idea behind Vertical Slice Architecture is that each feature owns its complete workflow.

Instead of:

```
Controllers
Services
Repositories
Validators
Handlers
```

the project is organized like:

```
Features
│
├── Create
│   ├── Request
│   ├── Command
│   ├── Validator
│   ├── Handler
│   └── Endpoint
│
├── Update
│
├── Delete
│
└── GetAll
```

Each slice contains the code needed for that specific use case.

---

# Solution Structure

```
Src
└── VerticalSliceArchitecture.Api

    ├── Features
    │   └── ToDos
    │       ├── Create
    │       ├── Update
    │       ├── Delete
    │       ├── GetAll
    │       ├── GetById
    │       └── ToggleComplete
    │
    ├── Entities
    ├── Data
    ├── Common
    ├── ExceptionHandling
    └── Program.cs
```

---

# Feature Organization

Each feature represents a business capability.

For example, creating a task contains everything required:

```
Create
│
├── Request
├── Command
├── Validator
├── Handler
└── Endpoint
```

The endpoint receives the request, the command represents the operation, the validator checks input rules, and the handler executes the use case.

This keeps the complete flow of a feature easy to discover.

---

# Example Features

## Create ToDo

Responsible for creating a new task.

Contains:

* Input contract
* Command
* Validation rules
* Handler logic
* API endpoint

---

## Update ToDo

Handles updating an existing task.

The feature owns its own workflow without depending on a shared service layer.

---

## Toggle Complete

Represents changing the completion state of a task.

The related behavior stays close to the feature instead of being scattered across multiple layers.

---

# Domain Model

This project intentionally uses a simple domain.

The main entity is:

## ToDo

A task contains:

* Title
* Description
* Completion status
* Completion date

The purpose of this entity is not to demonstrate complex domain modeling.

The focus of this repository is demonstrating how application behavior can be organized around features.

---

# Request Flow

A typical request follows this flow:

```
HTTP Request

    ↓

Carter Endpoint

    ↓

MediatR Command / Query

    ↓

Validator

    ↓

Handler

    ↓

Database
```

Each feature owns this complete flow.

---

# Technology Stack

* ASP.NET Core
* C#
* Entity Framework Core
* SQL Server
* MediatR
* Carter
* FluentValidation
* Vertical Slice Architecture

---

# Vertical Slice vs Layered Architecture

Both approaches solve different problems.

## Layered Architecture

Focuses on organizing code by technical responsibility:

* Controllers
* Services
* Repositories
* Infrastructure

## Vertical Slice Architecture

Focuses on organizing code by business capability:

* Create Task
* Update Task
* Delete Task
* Search Tasks

Vertical Slice Architecture can reduce coupling between unrelated features and make changes more localized.

---

# Scope and Limitations

This repository focuses on demonstrating the structure and concepts of Vertical Slice Architecture.

It intentionally avoids unnecessary complexity.

The project does not demonstrate:

* Large enterprise domain modeling
* Complex distributed systems
* Microservices communication
* Advanced event-driven architecture

The goal is to provide a clear and practical example of feature-based application organization.

---

# Running the Project

Clone the repository:

```bash
git clone https://github.com/Soheil-Ssh/aspnetcore-vertical-slice-architecture.git
```

Restore dependencies:

```bash
dotnet restore
```

Update database:

```bash
dotnet ef database update
```

Run the API:

```bash
dotnet run --project src/VerticalSliceArchitecture.Api
```

---

# Related Projects

This repository is part of a collection of ASP.NET Core architecture examples:

* [Clean Architecture](https://github.com/Soheil-Ssh/aspnetcore-clean-architecture)
* [Clean Architecture + DDD](https://github.com/Soheil-Ssh/aspnetcore-clean-architecture-with-ddd)
* [Vertical Slice Architecture](https://github.com/Soheil-Ssh/aspnetcore-vertical-slice-architecture)

Each repository focuses on a different architectural approach and its trade-offs.

## Contributing

Contributions, suggestions, and constructive feedback are welcome.

To contribute:

1. Fork the repository.
2. Create a new branch from the default branch.
3. Make a focused change.
4. Follow the existing project structure and coding conventions.
5. Add or update relevant tests when applicable.
6. Ensure the solution builds successfully.
7. Open a pull request with a clear description of the change.

For significant architectural changes, opening an issue before implementation is recommended.

## License

This project is licensed under the [MIT License](LICENSE).

You are free to use, modify, and distribute this project in accordance with the license terms.

## Author

Created and maintained by [Soheil-Ssh](https://github.com/Soheil-Ssh).