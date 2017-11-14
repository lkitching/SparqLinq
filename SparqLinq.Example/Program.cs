using SparqLinq;
using SparqLinq.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using VDS.RDF.Query.Datasets;
using VDS.RDF.Query.Patterns;

namespace SparqLinq.Example
{
    [Type("http://test/person")]
    public class Person
    {
        [Id]
        public Uri Uri { get; set; }

        [Predicate("http://test/name")]
        public string Name { get; set; }

        [Predicate("http://test/age")]
        public int Age { get; set; }

        public override string ToString()
        {
            return string.Format("Uri = {0}, Name = {1}, Age = {2}", this.Uri, this.Name, this.Age);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.Error.WriteLine("Usage: SparqLinq.Example datafile");
                Environment.Exit(1);
            }

            string dataFile = args[0];
            var store = LoadData(dataFile);
            //var ds = new InMemoryDataset(store, false);
            //var queryProcessor = new LeviathanQueryProcessor(ds);
            
            //var sparqlParser = new SparqlQueryParser();
            //var query = sparqlParser.ParseFromString("PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> SELECT * WHERE { ?s rdf:a <http://blah> }");
            //query.RootGraphPattern.TriplePatterns.Add(new TriplePattern(new VariablePattern("s"), new VariablePattern("p"), new VariablePattern("o")));
            //query.RootGraphPattern.TriplePatterns.Add(new TriplePattern(
            //    new VariablePattern("s"),
            //    new NodeMatchPattern(query.CreateUriNode(new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#type"))),
            //    new NodeMatchPattern(query.CreateUriNode(new Uri("http://blah")))));
            //query.RootGraphPattern.TriplePatterns.Add(new TriplePattern()

            //var result = (SparqlResultSet)queryProcessor.ProcessQuery(query);

            //foreach(var t in result)
            //{
            //    Console.WriteLine("o = {0}", t["o"]);
            //}

            var context = new SparqlContext(store);
            IEnumerable<Triple> triples = context.Default;

            foreach(var t in triples)
            {
                Console.WriteLine("s = {0}, p = {1}, o = {2}", t.Subject, t.Predicate, t.Object);
            }
        }

        private static TripleStore LoadData(string dataFile)
        {
            TripleStore ts = new TripleStore();
            ts.Add(LoadPeople(dataFile));
            return ts;
        }

        private static IGraph LoadPeople(string dataFile)
        {
            IGraph g = new Graph();
            TurtleParser p = new TurtleParser();
            p.Load(g, dataFile);
            g.BaseUri = null;
            return g;
        }
    }
}
