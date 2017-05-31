namespace ElasticSearchLite.NetCore.Interfaces
{
    public interface IStatementFactory
    {
        string Generate(IQuery query);
        Newtonsoft.Json.Serialization.NamingStrategy NamingStrategy { get; set; }
    }
}
