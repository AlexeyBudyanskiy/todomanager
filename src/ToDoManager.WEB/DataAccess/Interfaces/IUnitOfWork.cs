using System;
using ToDoManager.WEB.DataAccess.Entities;

namespace ToDoManager.WEB.DataAccess.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Assignment> Assignments { get; }

        void Save();
    }
}
