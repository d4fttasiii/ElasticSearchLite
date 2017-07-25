namespace ElasticSearchLite.NetCore.Interfaces.Bool
{
    public interface IBoolQuerySortAdded<TPoco> : IQuery where TPoco : IElasticPoco
    {
        IBoolQuerySortOrderDefined<TPoco> Ascending();
        IBoolQuerySortOrderDefined<TPoco> Descending();
    }
}
