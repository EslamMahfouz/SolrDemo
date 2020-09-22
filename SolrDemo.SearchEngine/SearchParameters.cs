using System.Collections.Generic;
namespace SolrDemo.SearchEngine
{
    public class SearchParameters
    {
        public SearchParameters()
        {
            SearchFor = new Dictionary<string, string>();
            Exclude = new Dictionary<string, string>();
            SortBy = new List<SortQuery>();
            FilterBy = new List<FilterQuery>();
            PageSize = DefaultPageSize;
            PageIndex = 1;
        }

        public const int DefaultPageSize = 4;
        public string FreeSearch { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public IDictionary<string, string> SearchFor { get; set; }
        public IDictionary<string, string> Exclude { get; set; }
        public IList<SortQuery> SortBy { get; set; }
        public IList<FilterQuery> FilterBy { get; set; }
    }
}
