namespace ElasticSearchLite.NetCore.Interfaces.Bool
{
    public interface IBoolQueryFragmentsLimited<TPoco>
        where TPoco: IElasticPoco
    {
        IBoolQueryExecutable<TPoco> LimitFragmentSizeTo(int fragmentSize);
    }
}
