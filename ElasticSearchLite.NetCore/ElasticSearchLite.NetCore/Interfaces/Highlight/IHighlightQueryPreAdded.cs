namespace ElasticSearchLite.NetCore.Interfaces.Highlight
{
    public interface IHighlightQueryPreAdded<TPoco> where TPoco : IElasticPoco
    {
        IHighlightQueryPostAdded<TPoco> WithPost(string postTag);
    }
}
