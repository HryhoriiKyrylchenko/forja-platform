namespace Forja.Domain.Repositories;

/// <summary>
/// Represents a unit of work interface specific to user profile management.
/// Provides access to user profile-related repositories and coordinates transactional operations.
/// </summary>
public interface IUserProfileUnitOfWork : IUnitOfWork
{
    /// <summary>
    /// Provides access to the operations related to Achievements in the user profile domain.
    /// It acts as the repository interface to manage Achievement entities.
    /// </summary>
    IAchievementRepository Achievements { get; }

    /// <summary>
    /// Gets the repository interface for managing user reviews.
    /// This property provides access to operations for retrieving, adding, updating, and deleting reviews
    /// in the context of the user profile domain.
    /// </summary>
    IReviewRepository Reviews { get; }

    /// <summary>
    /// Represents the repository for managing user achievement data in the system.
    /// </summary>
    /// <remarks>
    /// This property provides access to the <see cref="IUserAchievementRepository"/> repository,
    /// which is responsible for operations such as retrieving, adding, updating, or deleting user achievements.
    /// </remarks>
    IUserAchievementRepository UserAchievements { get; }

    /// <summary>
    /// Gets the repository for managing user library addons. This repository provides access to
    /// CRUD operations and query methods related to user-owned game addons within a user profile context.
    /// </summary>
    IUserLibraryAddonRepository UserLibraryAddons { get; }

    /// <summary>
    /// Represents the repository interface for managing operations
    /// related to user-owned games within the user's library.
    /// </summary>
    /// <remarks>
    /// This property provides access to functions such as retrieving,
    /// adding, updating, and deleting games that are associated with
    /// a user's library in the database.
    /// </remarks>
    IUserLibraryGameRepository UserLibraryGames { get; }

    /// <summary>
    /// Represents the repository for managing user-related data and operations within the UserProfile unit of work context.
    /// Provides methods for retrieving, adding, updating, and deleting user entities, as well as verifying user existence by username.
    /// </summary>
    IUserRepository Users { get; }
}