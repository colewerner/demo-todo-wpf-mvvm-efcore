using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Todo.Database.Factory
{
    public class TodoDbContextFactory : ITodoDbContextFactory
    {
        /// <summary>
        /// Creates a default db context
        /// </summary>
        /// <returns></returns>
        public TodoDbContext CreateContext()
        {
            return new TodoDbContext();
        }

        /// <summary>
        /// Creates a context factory with default options
        /// </summary>
        /// <param name="connectionString">Connection string to use when creating the <see cref="TodoDbContext"/></param>
        /// <returns></returns>
        public TodoDbContext CreateContext(string connectionString)
        {
            return new TodoDbContext(new DbContextOptionsBuilder<TodoDbContext>().UseSqlServer(connectionString)
                .EnableSensitiveDataLogging()
                .Options);
        }

        /// <summary>
        /// Create a <see cref="TodoDbContext"/> with <see cref="QueryTrackingBehavior.NoTracking"/>
        /// </summary>
        /// <param name="connectionString">Connection string to use when creating the <see cref="TodoDbContext"/></param>
        /// <returns></returns>
        public TodoDbContext CreateContextNoTracking(string connectionString)
        {
            return new TodoDbContext(new DbContextOptionsBuilder<TodoDbContext>().UseSqlServer(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .EnableSensitiveDataLogging()
                .Options);
        }
    }
}
