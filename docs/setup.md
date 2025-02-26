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
    dotnet user-secrets set Parameters:minio-root-user <root-user>
    dotnet user-secrets set Parameters:minio-root-password <password>
```

Add configuration in appsettings.json file in Forja.API project

```
"Keycloak": {
    "BaseUrl": "http://localhost:8080",
    "Realm": <realm>, //"forja"
    "ClientId": <client-id>, //"Forja.Api"
    "ClientUUID": <client-uuid>,
    "ClientSecret": <secret>
  }
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

***Setting up a Client in Keycloak***

- Log in to the Keycloak Admin Panel
    Open a browser and go to:http://localhost:8080/admin/
    Enter the administrator credentials:
    Username: keycloak-admin
    Password: keycloak-password

- Creating a Client
    In the left menu, find Clients.
    Click Create client (button in the upper right corner).

- Configuring the Client
    Fill in the Create Client form:
    Client ID: forja-api-client (or any unique name)
    Client authentication: Enabled
    Standard flow Enabled: Enabled
    Direct Access Grants Enabled: Enabled
    Service accounts roles: Enabled
    Root URL: http://localhost:3000 (or your frontend URL)
    Click Save.

- Retrieving Client Secret
    Navigate to the Credentials tab.
    Copy the Client Secret value – it will be needed in the backend.

- Adding Configuration to Forja.API
    Ensure the correct configuration in Forja.API\appsettings.json:

    `{
        "Keycloak": {
            "BaseUrl": "http://localhost:8080",
            "Realm": "my_realm",
            "ClientId": "forja-api-client",
            "ClientSecret": "your-secret-key"
        }
    }`
    
    Replace your-secret-key with the retrieved Client Secret.

- Assigning Roles to the Client
    Go to Users.
    Find Service account forja-api-client.
    Open the Role Mappings tab.
    Click Assign role.
    Select realm-management and assign the following role:
    manage-users (allows user creation).
  
- Go to Authentication in the left panel.
    Locate the "Verify Profile" option.
    Switch "Verify Profile" to Disable.

Setup is complete!

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
