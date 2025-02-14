## Project Setup Guide

### Prerequisites

- **Backend**: .NET 9 SDK, PostgreSQL, Redis, HashiCorp Vault
- **Frontend**: Node.js, npm/yarn, React
- **Authentication**: Keycloak configured
- **Storage**: MinIO running

### Installation

1. **Clone Repository**

```bash
    git clone https://github.com/HryhoriiKyrylchenko/forja-platform.git
    cd forja-platform
```

2. **Setup HashiCorp Vault**

Download docker image.

```bash
    docker pull hashicorp/vault:1.18
```

When you start the Vault server in dev mode, Vault UI is automatically enabled at http://127.0.0.1:8200/ui and ready to use.

When you first start the Vault server you need to create access tockens. You can do it with UI which mentioned above.
Save this tockens, you need them to access to the vault. 

For more information follow the instructions in https://hub.docker.com/r/hashicorp/vault.
Quick start for developers https://developer.hashicorp.com/vault/docs/get-started/developer-qs.

3. **Setup dotnet secrets**

Init user secrets in Forja.AppHost

```bash
    cd forja-platform/src/Aspire/Forja.AppHost

    dotnet user-secrets init

    dotnet user-secrets set Parameters:postgresql-password <password>
    dotnet user-secrets set Parameters:keycloak-admin <admin>
    dotnet user-secrets set Parameters:keycloak-password <password>
    dotnet user-secrets set Parameters:root-user <root-user>
    dotnet user-secrets set Parameters:root-password <password>
```

4. **Setup Postgres**

Download docker image.

```bash
    docker pull postgres:17.2
```

Create manually in your postgres new database `forjadb`.
Create manually in your postgres new database `keycloakdb`.

5. **Setup Redis**

Download docker image.

```bash
    docker pull redis:7.4
```

6. **Setup Keycloak**

Download docker image.

```bash
    docker pull keycloak/keycloak:26.1
```

Run Keycloak.
- Create a Realm: Your system will operate within a specific realm (e.g., forja).
- Configure the created realm.

7. **Setup MinIO**

Download docker image.

```bash
    docker pull minio/minio:RELEASE.2025-01-20T14-49-07Z
```

8. **Setup Backend**

```bash
    cd forja-platform/src/Api/Forja.Api
    dotnet restore
    dotnet build
```
9. **Setup Frontend**

```bash
    cd forja-platform/src/Web/forja-react
    npm install
    
    npm install keycloak-js
```

10. **Run**

***With .Net Aspire***

```bash
    cd forja-platform/src/Aspire/Forja.AppHost
    dotnet run
```
