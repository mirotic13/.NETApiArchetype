# ApiArchetype

## Overview

ApiArchetype is a template project for building RESTful APIs using .NET 8. This archetype includes a structured architecture and best practices to help you get started quickly and efficiently.

## Features

- **Clean Architecture**: Follows a Controller-Service-Repository pattern for better separation of concerns.
- **Entity Framework Core**: Easy database integration and data access.
- **Unit Testing**: Includes a dedicated project for unit tests using xUnit.
- **JWT Authentication**: Secure your API with JSON Web Token (JWT) authentication.
- **Minimal Dependencies**: Focuses on essential packages to keep the project lightweight.


## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- A suitable IDE (e.g., [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/))

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/yourusername/ApiArchetype.git
   cd ApiArchetype
   
2. Restore dependencies:

   ```bash
   dotnet restore
   
3. Run the API:
   
   ```bash
   dotnet run --project ApiArchetype
   
4. Open your browser and navigate to http://localhost:5000 to access the API.

### Running Tests

- To run the unit tests, use the following command:

```bash
dotnet test ApiArchetype.UnitTests
