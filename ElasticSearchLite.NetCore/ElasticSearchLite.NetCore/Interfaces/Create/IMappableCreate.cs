namespace ElasticSearchLite.NetCore.Interfaces.Create
{
    public interface IMappableCreate : IQuery
    {
        IMappingAddedCreate AddMapping(string name, bool indexed = true);
    }
}
