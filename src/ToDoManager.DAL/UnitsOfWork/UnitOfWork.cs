using System;
using Microsoft.EntityFrameworkCore;
using ToDoManager.DAL.EF;
using ToDoManager.DAL.Entities;
using ToDoManager.DAL.Interfaces;
using ToDoManager.DAL.Repositories;

namespace ToDoManager.DAL.UnitsOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        private IRepository<Assignment> _assignmentRepository;
        private readonly ManagerContext _managerContext;

        private bool _disposed;

        public UnitOfWork(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ManagerContext>();
            optionsBuilder.UseSqlServer(connectionString);
            _managerContext = new ManagerContext(optionsBuilder.Options);
        }

        public IRepository<Assignment> Assignments => _assignmentRepository ?? (_assignmentRepository = new CommonRepository<Assignment>(_managerContext));

        public void Save()
        {
            _managerContext.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _managerContext.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
