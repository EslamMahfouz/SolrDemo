namespace SolrDemo.SearchEngine
{
    public class FilterQuery
    {
        public string FieldName { get; set; }
        public string Value { get; set; }
        public string DataType { get; set; }
        public string LowerLimit { get; set; }
        public string UpperLimit { get; set; }
    }
}
