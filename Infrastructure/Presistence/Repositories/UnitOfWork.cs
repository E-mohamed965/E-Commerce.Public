using DomainLayer.Contracts;
using DomainLayer.Models;
using presentation.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistence.Repositories
{
    public class UnitOfWork(StoreDbContext _dbContext) : IUnitOfWork
    {
        private readonly Dictionary<string, object> _repositories = new Dictionary<string, object>();
        public IGenericRepository<TEntity, TKey> GetRepositoryAsync<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var type = typeof(TEntity).Name;
            if (_repositories.ContainsKey(type))
            {
                return (IGenericRepository<TEntity, TKey>)_repositories[type];
            }
            var repository = new GenericRepository<TEntity, TKey>(_dbContext);
            _repositories.Add(type, repository);
            return repository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
