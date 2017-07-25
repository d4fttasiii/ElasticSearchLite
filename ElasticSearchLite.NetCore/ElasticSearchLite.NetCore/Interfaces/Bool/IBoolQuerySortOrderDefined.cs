namespace ElasticSearchLite.NetCore.Interfaces.Bool
{
    public interface IBoolQuerySortOrderDefined<TPoco> : IQuery where TPoco : IElasticPoco
    {
        IBoolQueryExecutable<TPoco> WithKeyword();
        IBoolQueryExecutable<TPoco> WithoutKeyword();
    }
}
