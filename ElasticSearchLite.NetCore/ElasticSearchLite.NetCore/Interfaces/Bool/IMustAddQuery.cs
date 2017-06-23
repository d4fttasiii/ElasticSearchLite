namespace ElasticSearchLite.NetCore.Interfaces.Bool
{
    public interface IMustAddQuery<TPoco> : IShouldAddQuery<TPoco> where TPoco: IElasticPoco { }
}
