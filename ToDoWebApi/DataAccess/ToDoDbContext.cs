using Microsoft.EntityFrameworkCore;

namespace ToDoWebApi.DataAccess
{
    public class ToDoDbContext : DbContext
    {
        public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ToDoItemModel> TodoItemModels { get; set; }
    }
}