namespace ElasticSearchLite.NetCore.Interfaces
{
    public interface IDeleteExecutable<TPoco> : IQuery where TPoco : IElasticPoco { }
}
