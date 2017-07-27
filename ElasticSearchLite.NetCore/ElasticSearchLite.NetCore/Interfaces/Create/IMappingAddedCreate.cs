using ElasticSearchLite.NetCore.Models.Enums;

namespace ElasticSearchLite.NetCore.Interfaces.Create
{
    public interface IMappingAddedCreate
    {
        IMappingWithTypeAddedCreate WithType(ElasticCoreFieldDataTypes type);
    }
}