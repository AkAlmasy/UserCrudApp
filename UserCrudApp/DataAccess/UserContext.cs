using Microsoft.EntityFrameworkCore;
using UserCrudApp.Models;

namespace UserCrudApp.DataAccess
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Todo> Todos { get; set; }

    }
}
