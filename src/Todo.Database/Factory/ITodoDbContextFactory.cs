using Microsoft.EntityFrameworkCore;

namespace Todo.Database.Factory;

public interface ITodoDbContextFactory
{
    /// <summary>
    /// Creates a default db context
    /// </summary>
    /// <returns></returns>
    TodoDbContext CreateContext();

    /// <summary>
    /// Creates a context factory with default options
    /// </summary>
    /// <param name="connectionString">Connection string to use when creating the <see cref="TodoDbContext"/></param>
    /// <returns></returns>
    TodoDbContext CreateContext(string connectionString);

    /// <summary>
    /// Create a <see cref="TodoDbContext"/> with <see cref="QueryTrackingBehavior.NoTracking"/>
    /// </summary>
    /// <param name="connectionString">Connection string to use when creating the <see cref="TodoDbContext"/></param>
    /// <returns></returns>
    TodoDbContext CreateContextNoTracking(string connectionString);
}