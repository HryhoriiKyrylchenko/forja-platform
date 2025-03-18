using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace Forja.Infrastructure.Http.Data
{
    public class ForjaDbContextFactory : IDesignTimeDbContextFactory<ForjaDbContext>
    {
        public ForjaDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .AddUserSecrets<ForjaDbContextFactory>()
                .Build();

            var connectionString = configuration.GetConnectionString("forjaDb");
            var postgresPassword = configuration["Parameters:postgresql-password"];

            if (!string.IsNullOrEmpty(postgresPassword))
            {
                connectionString += $";Password={postgresPassword}";            }

            var optionsBuilder = new DbContextOptionsBuilder<ForjaDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new ForjaDbContext(optionsBuilder.Options);
        }
    }
}
