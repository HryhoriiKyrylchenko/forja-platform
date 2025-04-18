var builder = DistributedApplication.CreateBuilder(args);

//Postgres Configuration
var postgresPassword = builder.AddParameter("postgresql-password", secret: true);

builder.AddPostgres("postgres", password: postgresPassword)
    .WithImage("postgres")
    .WithImageTag("17.2")
    .WithContainerName("forja-postgres")
    .WithVolume("postgres-data", "/var/lib/postgresql/data", isReadOnly: false)
    .WithEndpoint(name: "postgresendpoint",
        scheme: "tcp",
        port: 5432,
        targetPort: 5432,
        isProxied: false)
    .WithLifetime(ContainerLifetime.Persistent);

    //Redis Configuration
    var redis = builder.AddRedis("redis")
        .WithImage("redis")
        .WithImageTag("7.4")
        .WithContainerName("forja-redis")
        .WithEndpoint(name: "redisendpoint",
            scheme: "tcp",
            port: 6379,
            targetPort: 6379,
            isProxied: false)
        .WithVolume("redis-data", "/data", isReadOnly: false)
        .WithLifetime(ContainerLifetime.Persistent);

//Keycloak Configuration
var keycloakUsername = builder.AddParameter("keycloak-admin", secret: true);
var keycloakPassword = builder.AddParameter("keycloak-password", secret: true);

builder.AddKeycloak("keycloak", 8080, keycloakUsername, keycloakPassword)
    .WithImage("keycloak/keycloak")
    .WithImageTag("26.1")
    .WithContainerName("forja-keycloak")
    .WithEnvironment("KC_DB", "postgres")
    .WithEnvironment("KC_DB_URL", "jdbc:postgresql://forja-postgres:5432/keycloakdb")
    .WithEnvironment("KC_DB_USERNAME", "postgres")
    .WithEnvironment("KC_DB_PASSWORD", postgresPassword)
    .WithEnvironment("KC_HOSTNAME", "localhost")
    .WithEnvironment("KC_DB_POOL_INITIAL_SIZE", "5") 
    .WithEnvironment("KC_DB_POOL_MAX_SIZE", "20")    
    .WithLifetime(ContainerLifetime.Persistent);

//MinIO Configuration
var minioRootUser = builder.AddParameter("minio-root-user", secret: true);
var minioRootPassword = builder.AddParameter("minio-root-password", secret: true);

builder.AddContainer("minio", "minio/minio")
    .WithImage("minio/minio")
    .WithImageTag("RELEASE.2025-01-20T14-49-07Z")
    .WithContainerName("forja-minio")
    .WithHttpEndpoint(port: 9000, targetPort: 9000, name: "minio-container-port")
    .WithHttpEndpoint(port: 9001, targetPort: 9001, name: "minio-console-port")
    .WithEnvironment("MINIO_ROOT_USER", minioRootUser)
    .WithEnvironment("MINIO_ROOT_PASSWORD", minioRootPassword)
    .WithEnvironment("MINIO_ADDRESS", ":9000")
    .WithEnvironment("MINIO_CONSOLE_ADDRESS", ":9001")
    .WithArgs("server", "/data")
    .WithVolume("minio-data", "/data", isReadOnly: false)
    .WithLifetime(ContainerLifetime.Persistent);

//backend
var forjaApi = builder.AddProject<Projects.Forja_API>("forjaapi")
    .WithExternalHttpEndpoints()
    .WithReference(redis);

//frontend
builder.AddNpmApp("forja-react", "../../Web/forja-react")
    .WaitFor(forjaApi);

builder.Build().Run();