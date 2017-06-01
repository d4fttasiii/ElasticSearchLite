using ElasticSearchLite.NetCore.Models;

namespace ElasticSearchLite.NetCore.Interfaces.Search
{
    public interface IMatchFilteringQuery<TPoco> : IExecutableSearchQuery<TPoco> where TPoco : IElasticPoco
    {
        /// <summary>
        /// Setting up the match operator
        /// </summary>
        /// <param name="op">Operator type between match tokens (and, or)</param>
        /// <returns></returns>
        IFilteredSearchQuery<TPoco> Operator(ElasticOperators op = null);
    }
}
