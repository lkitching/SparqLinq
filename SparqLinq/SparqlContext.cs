using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;

namespace SparqLinq
{
    public class SparqlContext
    {
        private readonly TripleStore store;
        public SparqlContext(TripleStore store)
        {
            this.store = store;
        }

        public IQueryable<Triple> Graph(Uri graphUri)
        {
            return new GraphQueryable(graphUri, new SparqlQueryProvider(this.store));
        }

        public IQueryable<Triple> Default
        {
            get { return Graph(null); }
        }
    }
}
