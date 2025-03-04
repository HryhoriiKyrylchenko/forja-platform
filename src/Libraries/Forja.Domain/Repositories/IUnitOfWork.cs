namespace Forja.Domain.Repositories;

/// <summary>
/// Defines the UnitOfWork pattern interface for managing transactions
/// and coordinating changes across multiple repositories.
/// </summary>
[Obsolete]
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Asynchronously saves all changes made in the context to the database.
    /// </summary>
    /// <returns>A task representing the asynchronous save operation.</returns>
    Task SaveChangesAsync();
}