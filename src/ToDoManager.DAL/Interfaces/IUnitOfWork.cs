using System;
using ToDoManager.DAL.Entities;

namespace ToDoManager.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Assignment> Assignments { get; }

        void Save();
    }
}
