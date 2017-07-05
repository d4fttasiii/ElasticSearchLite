namespace ElasticSearchLite.NetCore.Interfaces.Highlight
{
    public interface IHighlightQueryUnconfigured<TPoco> where TPoco : IElasticPoco
    {
        IHighlightQueryPreAdded<TPoco> WithPre(string preTag);
    }
}
