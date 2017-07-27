using ElasticSearchLite.NetCore.Models.Enums;

namespace ElasticSearchLite.NetCore.Interfaces.Create
{
    public interface IMappingWithTypeAddedCreate : IMappableCreate
    {
        IFieldAnalyserAddedCreate AddFieldAnalyzer(ElasticAnalyzers analyzer);
    }
}
