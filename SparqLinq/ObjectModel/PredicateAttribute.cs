using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparqLinq.ObjectModel
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PredicateAttribute : Attribute
    {
        public PredicateAttribute(string predicateUri)
        {
            this.Predicate = new Uri(predicateUri);
        }

        public Uri Predicate { get; private set; }
    }
}
