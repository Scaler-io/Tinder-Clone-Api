using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Tinder_Dating_API.DataAccess.Interfaces;

namespace Tinder_Dating_API.DataAccess.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification() { }
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
        public Expression<Func<T, bool>> Criteria { get; }

        public List<string> IncludeStrings { get; } = new List<string>();

        public Expression<Func<T, object>> OrderBy {get; private set; }

        public Expression<Func<T, object>> OrderByDescending {get; private set; }

        public int Take {get; private set; }

        public int Skip {get; private set; }

        public bool IsPagingEnabled {get; private set; }

        protected void AddIncludes(string includeString)
        {
            this.IncludeStrings.Add(includeString);
        }

        protected void AddOrderBy(Expression<Func<T, object>> orderBy)
        {
            this.OrderBy = orderBy;
        }

        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescending)
        {
            OrderByDescending = orderByDescending;
        }

        protected void ApplyPaging(int skip, int take)
        {
            Take = take;
            Skip = skip;
            IsPagingEnabled = true;
        }
    }
}
