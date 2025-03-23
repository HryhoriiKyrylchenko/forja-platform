try
{
    var builder = DistributedApplication.CreateBuilder(args);

    //Postgres Configuration
    var postgresPassword = builder.AddParameter("postgresql-password", secret: true);

    var postgres = builder.AddPostgres("postgres", password: postgresPassword)
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

    var keycloakDb = postgres.AddDatabase("keycloakdb");

    //Redis Configuration
    var redis = builder.AddRedis("redis")
        .WithImage("redis")
        .WithImageTag("7.4")
        .WithContainerName("forja-redis")
        .WithVolume("redis-data", "/data", isReadOnly: false);

    //Keycloak Configuration
    var keycloakUsername = builder.AddParameter("keycloak-admin", secret: true);
    var keycloakPassword = builder.AddParameter("keycloak-password", secret: true);

    var keycloak = builder.AddKeycloak("keycloak", 8080, keycloakUsername, keycloakPassword)
        .WithImage("keycloak/keycloak")
        .WithImageTag("26.1")
        .WithContainerName("forja-keycloak")
        .WithReference(keycloakDb).WaitFor(keycloakDb)
        .WithEnvironment("KC_DB", "postgres")
        .WithEnvironment("KC_DB_URL", "jdbc:postgresql://forja-postgres:5432/keycloakdb")
        .WithEnvironment("KC_DB_USERNAME", "postgres")
        .WithEnvironment("KC_DB_PASSWORD", postgresPassword)
        .WithEnvironment("KC_HOSTNAME", "localhost")
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
        .WithReference(redis)
        .WithReference(keycloak).WaitFor(keycloak);

    //frontend
    builder.AddNpmApp("forja-react", "../../Web/forja-react")
        .WaitFor(forjaApi);

    builder.Build().Run();
}
catch (Exception ex)
{
    Console.WriteLine($"-- Fatal error: {ex.Message}");
}