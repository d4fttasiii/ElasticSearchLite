namespace ElasticSearchLite.NetCore.Interfaces.Bool
{
    public interface IBoolQueryPreAdded<TPoco>
        where TPoco : IElasticPoco
    {
        IBoolQueryPostAdded<TPoco> SetPostTagTo(string postTag);
    }
}
