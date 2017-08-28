namespace ElasticSearchLite.NetCore.Interfaces.Bool
{
    public interface IBoolQueryPostAdded<TPoco>
        where TPoco : IElasticPoco
    {
        IBoolQueryFragmentsLimited<TPoco> LimitTheNumberOfFragmentsTo(int numberOfFragments);
    }
}
