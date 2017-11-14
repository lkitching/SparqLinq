using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;

namespace SparqLinq
{
    public class GraphQueryable : IQueryable<Triple>
    {
        public GraphQueryable(Uri graphUri, IQueryProvider provider)
        {
            this.GraphUri = graphUri;
            this.Provider = provider;
        }

        public Uri GraphUri { get; private set; }

        public Type ElementType
        {
            get
            {
                return typeof(Triple);
            }
        }

        public Expression Expression
        {
            get
            {
                return Expression.Constant(this);
            }
        }

        public IQueryProvider Provider { get; private set; }

        public IEnumerator<Triple> GetEnumerator()
        {
            return this.Provider.Execute<IEnumerable<Triple>>(this.Expression).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
