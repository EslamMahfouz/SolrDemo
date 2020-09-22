using SolrDemo.Common;
using SolrNet;

namespace SolrDemo.SearchEngine
{
    public class SearchResult<T>
    {
        public int TotalResults { get; set; }
        public SolrQueryResults<Post> QueryResults { get; set; }
        public SearchResult(SolrQueryResults<Post> matchingPosts)
        {
            QueryResults = matchingPosts;
            TotalResults = matchingPosts.NumFound;
        }
    }
}
