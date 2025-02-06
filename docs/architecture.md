## Architecture Overview

### System Architecture

Forja is designed as a modular and scalable gaming platform inspired by battle.net. The system follows a microservices-inspired monolithic architecture, where the backend is built using .NET and communicates with various services like authentication, storage, and caching.

### High-Level Components

- **Frontend (React)**: The client application, providing a responsive and interactive user experience.
- **Backend (.NET)**: Handles business logic, API requests, and communication with external services.
- **Authentication (Keycloak)**: Manages user authentication and authorization.
- **Storage (MinIO)**: S3-compatible object storage for user content.
- **Database (PostgreSQL)**: Stores relational data for users, transactions, and game details.
- **Caching (Redis)**: Enhances performance by caching frequently accessed data.
- **Secrets Management (HashiCorp Vault)**: Securely manages API keys, database credentials, and sensitive information.

### Service Communication

- The frontend communicates with the backend through REST APIs.
- The backend integrates with PostgreSQL, MinIO, and Redis to manage data.
- Keycloak handles authentication and token validation.
- Redis is used for session storage and frequently accessed queries.
- HashiCorp Vault manages sensitive credentials securely.

### Diagram

[Frontend (React)]  <--->  [Backend (.NET)]  <--->  [PostgreSQL, MinIO, Redis, Keycloak, Vault]
