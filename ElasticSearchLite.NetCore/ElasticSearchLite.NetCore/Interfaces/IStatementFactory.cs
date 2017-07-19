using Newtonsoft.Json.Serialization;

namespace ElasticSearchLite.NetCore.Interfaces
{
    public interface IStatementFactory
    {
        string Generate(IQuery query);
        NamingStrategy NamingStrategy { get; set; }
        DefaultContractResolver ContractResolver { get; set; }
    }
}
