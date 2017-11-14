using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF.Query;

namespace SparqLinq.ObjectModel
{
    public interface ITypeMapper
    {
        object FromResult(SparqlResult result);
    }
}
