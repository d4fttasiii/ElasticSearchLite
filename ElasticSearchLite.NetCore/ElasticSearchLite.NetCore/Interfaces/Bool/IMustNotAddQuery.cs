namespace ElasticSearchLite.NetCore.Interfaces.Bool
{
    public interface IMustNotAddQuery<TPoco> : IShouldAddQuery<TPoco> where TPoco : IElasticPoco { }
}
