using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SparqLinq
{
    public class SparqlQueryable<T> : IQueryable<T>
    {
        private readonly SparqlQueryProvider provider;
        private readonly Expression expr;

        public SparqlQueryable(SparqlQueryProvider provider, Expression queryExpression)
        {
            this.provider = provider;
            this.expr = queryExpression;
        }

        public Type ElementType
        {
            get { return typeof(T); }
        }

        public Expression Expression
        {
            get { return this.expr; }
        }

        public IQueryProvider Provider
        {
            get { return this.provider; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            var seq = this.provider.Execute<IEnumerable<T>>(this.expr);
            return seq.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
