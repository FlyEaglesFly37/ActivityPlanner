using Microsoft.EntityFrameworkCore;
 
namespace Belt.Models
{
    public class UserContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
        public DbSet<User> Users {get; set;}
        public DbSet<Todo> Todos {get; set;}
        public DbSet<Guest> Guests {get; set;}
    }
}
