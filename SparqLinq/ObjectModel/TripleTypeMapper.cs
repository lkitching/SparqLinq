using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Query;

namespace SparqLinq.ObjectModel
{
    public class TripleTypeMapper : ITypeMapper
    {
        public object FromResult(SparqlResult result)
        {
            return new Triple(result["s"], result["p"], result["o"]);
        }
    }
}
