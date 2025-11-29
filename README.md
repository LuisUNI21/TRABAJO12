# Task Manager API

## Overview
The Task Manager API is a RESTful web service built using .NET Core 8 for managing tasks. It provides a complete CRUD (Create, Read, Update, Delete) functionality for task management, allowing users to create, retrieve, update, and delete tasks with various statuses.

## Features
- **Task Management**: Create, read, update, and delete tasks.
- **Task Status**: Supports multiple statuses for tasks: Draft, ToDo, InProgress, and Done.
- **Database**: Utilizes SQL Server for data persistence with Entity Framework Core.
- **API Documentation**: Integrated with Swagger for easy API exploration and testing.
- **Dockerized**: The application is containerized using Docker Compose for easy deployment.

## Project Structure
```
task-manager-api
├── src
│   ├── TaskManager.Api          # API project
│   ├── TaskManager.Application   # Application layer
│   ├── TaskManager.Domain        # Domain layer
│   └── TaskManager.Infrastructure # Infrastructure layer
├── docker                        # Docker configuration
├── TaskManager.sln              # Solution file
└── README.md                    # Project documentation
```

## Getting Started

### Prerequisites
- .NET Core 8 SDK
- Docker and Docker Compose

### Setup Instructions

1. **Clone the Repository**
   ```bash
   git clone <repository-url>
   cd task-manager-api
   ```

2. **Build the Docker Containers**
   ```bash
   docker-compose up --build
   ```

3. **Access the API**
   The API will be available at `http://localhost:5000`. You can access the Swagger UI at `http://localhost:5000/swagger`.

4. **Database Initialization**
   The SQL Server database will be initialized with the necessary schema and seed data as defined in `docker/sqlserver/init.sql`.

### Usage
- Use the Swagger UI to interact with the API endpoints.
- You can create tasks, update their status, and delete them as needed.

## Contributing
Contributions are welcome! Please submit a pull request or open an issue for any enhancements or bug fixes.

## License
This project is licensed under the MIT License. See the LICENSE file for details.