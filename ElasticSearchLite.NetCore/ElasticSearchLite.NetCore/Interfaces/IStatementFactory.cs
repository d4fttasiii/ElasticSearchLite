namespace ElasticSearchLite.NetCore.Interfaces
{
    public interface IStatementFactory
    {
        string Generate(IQuery query);
    }
}
