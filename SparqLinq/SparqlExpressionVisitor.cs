using System;
using System.Linq;
using System.Linq.Expressions;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using VDS.RDF.Query.Patterns;

namespace SparqLinq
{
    public class SparqlExpressionVisitor : ExpressionVisitor
    {
        private readonly SparqlQuery query;

        public SparqlExpressionVisitor()
        {
            var sparqlParser = new SparqlQueryParser();
            this.query = sparqlParser.ParseFromString("PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> SELECT * WHERE { }");
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if(IsWhereMethodCall(node))
            {
                //first argument to Where is an expression representing the source being filtered
                //i.e. a type inside a graph
                this.Visit(node.Arguments[0]);

                //second argument is an expression to filter the source
                this.Visit(node.Arguments[1]);
            }
            else if(IsOfTypeMethodCall(node))
            {
                //first argument is the source to filter
                this.Visit(node.Arguments[0]);

                //get type mapping

                //
                query.RootGraphPattern.TriplePatterns.Add(new TriplePattern(
                    new VariablePattern("s"),
                    new NodeMatchPattern(query.CreateUriNode(new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#type"))),
                    new NodeMatchPattern(query.CreateUriNode(new Uri("http://test/person")))));
                //query.RootGraphPattern.TriplePatterns.Add(new TriplePattern(new VariablePattern("s"), new NodeMatchPattern(, new VariablePattern("o")));
            }
            else
            {
                throw new InvalidOperationException("Only Where and OfType expressions are supported");
            }

            return node;
        }

        private static bool IsWhereMethodCall(MethodCallExpression node)
        {
            return node.Method.DeclaringType == typeof(Queryable) && node.Method.Name == "Where";
        }

        private static bool IsOfTypeMethodCall(MethodCallExpression node)
        {
            return node.Method.DeclaringType == typeof(Queryable) && node.Method.Name == "OfType";
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch(node.NodeType)
            {
                case ExpressionType.Equal:
                    
                    throw new NotImplementedException();
                default:
                    throw new ArgumentException("Only == operator is supported");
            }
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            if(node.Value is GraphQueryable)
            {
                var g = (GraphQueryable)node.Value;
                if(g.GraphUri == null)
                {
                    this.query.RootGraphPattern.TriplePatterns.Add(new TriplePattern(new VariablePattern("s"), new VariablePattern("p"), new VariablePattern("o")));
                }
                else
                {
                    throw new NotImplementedException();
                    //sb.AppendFormat("SELECT * WHERE { GRAPH <{0}> { ?s ?p ?o ", g.GraphUri);
                }
            }
            return node;
        }

        public SparqlQuery Query
        {
            get { return this.query; }
        }
    }
}
