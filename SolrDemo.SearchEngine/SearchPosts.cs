using CommonServiceLocator;
using SolrDemo.Common;
using SolrNet;
using SolrNet.Commands.Parameters;
using System;
using System.Collections.Generic;

namespace SolrDemo.SearchEngine
{
    public static class SearchPosts
    {
        private static readonly ISolrReadOnlyOperations<Post> Solr;

        static SearchPosts()
        {
            Startup.Init<Post>("http://localhost:8983/solr/Tetco.SG");
            Solr = ServiceLocator.Current.GetInstance<ISolrReadOnlyOperations<Post>>();
        }

        public static SearchResult<Post> Search(SearchParameters parameters)

        {
            int start = 1;
            if (parameters.PageIndex > 0)
            {
                start = (parameters.PageIndex - 1) * parameters.PageSize;
            }

            var matchingPosts = Solr.Query(BuildQuery(parameters), new QueryOptions
            {
                FilterQueries = BuildFilterQueries(parameters),
                Rows = parameters.PageSize,
                StartOrCursor = new StartOrCursor.Start(start),
                OrderBy = GetSelectedSort(parameters)
            });
            return new SearchResult<Post>(matchingPosts);
        }

        public static ISolrQuery BuildQuery(SearchParameters parameters)
        {
            if (!string.IsNullOrEmpty(parameters.FreeSearch))
                return new SolrQuery(parameters.FreeSearch);

            AbstractSolrQuery searchQuery = null;
            List<SolrQuery> solrQuery = new List<SolrQuery>();
            List<SolrQuery> solrNotQuery = new List<SolrQuery>();

            foreach (var searchType in parameters.SearchFor)
            {
                solrQuery.Add(new SolrQuery($"{searchType.Key}:{searchType.Value}"));
            }

            if (solrQuery.Count > 0)
                searchQuery = new SolrMultipleCriteriaQuery(solrQuery, SolrMultipleCriteriaQuery.Operator.OR);

            foreach (var excludeType in parameters.Exclude)
            {
                solrNotQuery.Add(new SolrQuery($"{excludeType.Key}:{excludeType.Value}"));
            }

            if (solrNotQuery.Count > 0)
            {
                searchQuery = (searchQuery ?? SolrQuery.All) - new SolrMultipleCriteriaQuery(solrNotQuery, SolrMultipleCriteriaQuery.Operator.OR);
            }

            return searchQuery ?? SolrQuery.All;
        }

        public static ICollection<ISolrQuery> BuildFilterQueries(SearchParameters parameters)
        {
            List<ISolrQuery> filter = new List<ISolrQuery>();

            foreach (var filterBy in parameters.FilterBy)
            {
                if (!string.IsNullOrEmpty(filterBy.DataType))
                {
                    DateTime upperLimit = Convert.ToDateTime(filterBy.UpperLimit);
                    DateTime lowerLimit = Convert.ToDateTime(filterBy.LowerLimit);
                    if (upperLimit.Equals(lowerLimit))
                    {
                        upperLimit = upperLimit.AddDays(1);
                    }

                    filter.Add(new SolrQueryByRange<DateTime>(filterBy.FieldName, lowerLimit, upperLimit));
                }

                else
                {
                    if (filterBy.Value.Contains(";"))
                    {
                        var filterValues = filterBy.Value.Split(';');
                        List<SolrQueryByField> filterForPost = new List<SolrQueryByField>();
                        foreach (string filterVal in filterValues)
                        {
                            filterForPost.Add(new SolrQueryByField(filterBy.FieldName, filterVal) { Quoted = false });
                        }

                        filter.Add(new SolrMultipleCriteriaQuery(filterForPost, SolrMultipleCriteriaQuery.Operator.OR));
                    }

                    else
                    {
                        filter.Add(new SolrQueryByField(filterBy.FieldName, filterBy.Value));
                    }
                }
            }

            return filter;
        }

        private static ICollection<SortOrder> GetSelectedSort(SearchParameters parameters)
        {
            List<SortOrder> sortQueries = new List<SortOrder>();
            foreach (var sortBy in parameters.SortBy)
            {
                sortQueries.Add(sortBy.Order.Equals(Enums.SortOrder.Ascending)
                    ? new SortOrder(sortBy.FieldName, Order.ASC)
                    : new SortOrder(sortBy.FieldName, Order.DESC));
            }
            return sortQueries;
        }
    }
}
