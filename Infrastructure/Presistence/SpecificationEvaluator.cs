using DomainLayer.Contracts;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistence
{
    internal class SpecificationEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> inputQuery, ISpecification<TEntity, TKey> specification) where TEntity : BaseEntity<TKey>
        {
            var query = inputQuery;
            // Apply criteria
            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }
            // Apply sorting
            if (specification.OrderBy is not null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDescending is not null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }
            // Apply pagination
            if(specification.IsPaginated)
            {
                query = query.Skip(specification.Skip);
                query = query.Take(specification.Take);
            }
            // Apply includes
            if (specification.IncludeExpressions is not null && specification.IncludeExpressions.Any()) 
            { 
                //foreach (var include in specification.IncludeExpressions)
                //{
                //    query.Include(include);
                //}
                query = specification.IncludeExpressions.Aggregate(query, (current, include) => current.Include(include));  
            }
            
            return query;
        }
    }

}
