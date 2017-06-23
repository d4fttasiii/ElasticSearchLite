using ElasticSearchLite.NetCore.Models;

namespace ElasticSearchLite.NetCore.Interfaces
{
    public interface IElasticCondition
    {
        ElasticField Field { get; set; }
    }
}
