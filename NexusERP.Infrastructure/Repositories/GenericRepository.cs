using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using NexusERP.Application.Interfaces;
using NexusERP.Domain.Common;
using NexusERP.Infrastructure.Data;

namespace NexusERP.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T>
        where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet
                .Where(entity => !entity.IsDeleted)
                .ToList();
        }

        public T GetById(int id)
        {
            return _dbSet
                .FirstOrDefault(entity =>
                    entity.Id == id &&
                    !entity.IsDeleted);
        }

        public void Add(T entity)
        {
            entity.CreatedAt = DateTime.Now;
            entity.IsActive = true;
            entity.IsDeleted = false;

            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(
                    "entity");
            }

            var entry = _context.Entry(entity);

            /*
             * إذا كان الكيان جديدًا وتمت إضافته في العملية نفسها،
             * نترك حالته Added حتى ينفذ EF أمر INSERT.
             */
            if (entry.State == EntityState.Added)
            {
                return;
            }

            /*
             * إذا لم يكن الكيان متتبعًا، نربطه بالسياق
             * ثم نحدد أنه معدل.
             */
            if (entry.State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
                entry = _context.Entry(entity);
            }

            entry.State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            // حذف منطقي: لا نحذف السجل فعليًا من قاعدة البيانات
            entity.IsDeleted = true;
            entity.IsActive = false;
            entity.UpdatedAt = DateTime.Now;

            _context.Entry(entity).State =
                EntityState.Modified;
        }
    }
}