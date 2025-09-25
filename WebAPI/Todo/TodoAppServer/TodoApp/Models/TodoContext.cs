using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace TodoApp.Models
{
    public class TodoContext : DbContext
    {
        DbSet<TodoItem> TodoItems { get; set; }

        public TodoContext(DbContextOptions<TodoContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }
    }
}
