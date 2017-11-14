using SparqLinq.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VDS.RDF;
using VDS.RDF.Query;
using VDS.RDF.Query.Datasets;

namespace SparqLinq
{
    public class SparqlQueryProvider : IQueryProvider
    {
        private readonly TripleStore store;
        private readonly Dictionary<Type, ITypeMapper> typeMappings = new Dictionary<Type, ITypeMapper>();

        public SparqlQueryProvider(TripleStore store)
        {
            this.store = store;
            this.typeMappings[typeof(Triple)] = new TripleTypeMapper();
        }

        public IQueryable CreateQuery(Expression expression)
        {
            Type expressionType = expression.Type;
            Type qType = typeof(SparqlQueryable<>).MakeGenericType(expression.Type);
            return (IQueryable)Activator.CreateInstance(qType, new object[] { this, expression });
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new SparqlQueryable<TElement>(this, expression);
        }

        public object Execute(Expression expression)
        {
            var seqElementType = GetEnumerableElementType(expression.Type);

            if(seqElementType == null)
            {
                throw new ArgumentException("Only IEnumerable<> results currently supported");
            }

            ITypeMapper mapper = this.typeMappings[seqElementType];

            //visit expression to SPARQL query
            var visitor = new SparqlExpressionVisitor();
            visitor.Visit(expression);

            //execute query
            var ds = new InMemoryDataset(store, false);
            var queryProcessor = new LeviathanQueryProcessor(ds);

            var result = (SparqlResultSet)queryProcessor.ProcessQuery(visitor.Query);

            //map results
            var l = (System.Collections.IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(seqElementType));
            foreach(var r in result)
            {
                l.Add(mapper.FromResult(r));
            }

            return l;
        }

        private static Type GetEnumerableElementType(Type resultType)
        {
            var et = resultType.GetInterfaces().FirstOrDefault(it => it.IsGenericType && it.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            return et == null ? null : et.GetGenericArguments()[0];
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return (TResult)this.Execute(expression);
        }
    }
}
