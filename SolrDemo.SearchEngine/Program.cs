using System.Collections.Generic;

namespace SolrDemo.SearchEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            var posts = SearchPosts.Search(new SearchParameters
            {
                SearchFor = new Dictionary<string, string> { { "title", "for" } },
                SortBy = new List<SortQuery> { new SortQuery { FieldName = "id", Order = Enums.SortOrder.Ascending } }
            }).QueryResults;
        }
    }
}
