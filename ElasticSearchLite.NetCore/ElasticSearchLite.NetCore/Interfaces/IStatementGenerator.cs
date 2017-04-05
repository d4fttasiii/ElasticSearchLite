namespace ElasticSearchLite.NetCore.Interfaces
{
    public interface IStatementGenerator
    {
        string Generate(IQuery query);
    }
}
