using ElasticSearchLite.NetCore.Models;

namespace ElasticSearchLite.NetCore.Interfaces.Create
{
    public interface IFieldAnalyserAddedCreate
    {
        IMappableCreate WithConfiguration(ElasticAnalyzerConfiguration configuration);
        IMappableCreate WithoutConfiguration();
    }
}