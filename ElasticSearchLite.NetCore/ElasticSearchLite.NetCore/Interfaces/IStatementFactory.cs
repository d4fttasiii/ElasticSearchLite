using ElasticSearchLite.NetCore.Queries;

namespace ElasticSearchLite.NetCore.Interfaces
{
    public interface IStatementFactory
    {
        string Generate(AbstractQuery query);
    }
}
