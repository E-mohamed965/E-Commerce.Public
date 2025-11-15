using DomainLayer.Contracts;
using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    abstract class BaseSpecification<Tentity, TKey> : ISpecification<Tentity, TKey> where Tentity : BaseEntity<TKey>
    {
        protected BaseSpecification(Expression<Func<Tentity, bool>>? criteria)
        {
            Criteria = criteria;
          //  Includes = new List<Expression<Func<Tentity, object>>>();
        }

        public Expression<Func<Tentity, bool>> Criteria { get; private set; }

        #region Include
        public List<Expression<Func<Tentity, object>>> IncludeExpressions { get; } = new List<Expression<Func<Tentity, object>>>();

        protected void AddInclude(Expression<Func<Tentity, object>> includeExpression)
        {
            IncludeExpressions.Add(includeExpression);
        }
        #endregion

        #region Sorting
        public Expression<Func<Tentity, object>>? OrderBy { get; private set; }

        public Expression<Func<Tentity, object>>? OrderByDescending { get; private set; }


        protected void AddOrderBy(Expression<Func<Tentity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        protected void AddOrderByDescending(Expression<Func<Tentity, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }
        #endregion
        #region Pagination
        public int Take  {get; private set; }

        public int Skip { get; private set; }

        public bool IsPaginated {get; set; }
        protected void ApplyPagination(int PageSize, int PageIndex)
        {
            Skip = (PageIndex-1)*PageSize;
            Take = PageSize;
            IsPaginated = true;
        }
        #endregion

    }
}
