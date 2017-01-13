using Microsoft.EntityFrameworkCore;
using ToDoManager.WEB.DataAccess.Entities;

namespace ToDoManager.WEB.DataAccess.EF
{
    public class ManagerContext : DbContext
    {
        public ManagerContext()
        {
        }

        public ManagerContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Assignment> Assignments { get; set; }
    }
}
