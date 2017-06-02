using ElasticSearchLite.NetCore.Models;

namespace ElasticSearchLite.NetCore.Interfaces.Create
{
    public interface IMappingAddedCreate
    {
        IMappingWithTypeAddedCreate WithType(ElasticCoreFieldDataTypes type);
    }
}