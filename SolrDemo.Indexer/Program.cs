using CommonServiceLocator;
using SolrDemo.Common;
using SolrNet;
using System.Linq;
using System.Xml;

namespace SolrDemo.Indexer
{
    class Program
    {
        static void Main(string[] args)
        {
            Startup.Init<Post>("http://localhost:8983/solr/Tetco.SG");
            var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Post>>();
            using (XmlReader reader = XmlReader.Create(@"Posts.xml"))
            {
                while (reader.Read())
                {
                    if (reader.Name == "row")
                    {
                        Post post = new Post { Id = reader["Id"] };
                        if (reader["Title"] != null)
                        {
                            post.Title = reader["Title"];
                        }
                        if (reader["Tags"] != null)
                        {
                            post.Tags = reader["Tags"].Split(new char[] { '<', '>' })
                                .Where(t => !string.IsNullOrEmpty(t)).ToList();
                        }

                        solr.Add(post);
                    }
                }
            }
            solr.Commit();
        }
    }
}
