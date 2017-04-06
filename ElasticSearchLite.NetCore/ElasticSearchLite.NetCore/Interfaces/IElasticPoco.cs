namespace ElasticSearchLite.NetCore.Interfaces
{
    public interface IElasticPoco
    {
        string Id { get; set; }
        string Type { get; set; }
        string Index { get; set; }
        double Score { get; set; }
    }
}
