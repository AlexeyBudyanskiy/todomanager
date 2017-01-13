using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ToDoManager.DAL.EF;
using ToDoManager.DAL.Entities;
using ToDoManager.DAL.Interfaces;

namespace ToDoManager.DAL.Repositories
{
    public class CommonRepository<TEntity> : IRepository<TEntity> where TEntity : BaseType
    {
        protected readonly ManagerContext Db;

        public CommonRepository(ManagerContext db)
        {
            Db = db;
        }

        public IQueryable<TEntity> GetAll()
        {
            return Db.Set<TEntity>();
        }

        public TEntity Get(int id)
        {
            //return Db.Set<TEntity>().

            return Db.Set<TEntity>().Where(x=>x.Id == id).AsQueryable().FirstOrDefault();
        }

        public IQueryable<TEntity> Find(Func<TEntity, bool> predicate)
        {
            return Db.Set<TEntity>().Where(predicate).AsQueryable();
        }

        public virtual void Create(TEntity item)
        {
            Db.Set<TEntity>().Add(item);
        }

        public virtual void Update(TEntity item)
        {
            Db.Entry(item).State = EntityState.Modified;
        }

        public virtual void Delete(int id)
        {
            var item = Get(id);
            Db.Set<TEntity>().Remove(item);
        }
    }
}
