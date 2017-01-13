using Microsoft.EntityFrameworkCore;
using ToDoManager.DAL.Entities;

namespace ToDoManager.DAL.EF
{
    public class ManagerContext : DbContext
    {
        static ManagerContext()
        {
        }

        public ManagerContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Assignment> Assignments { get; set; }
    }
}
