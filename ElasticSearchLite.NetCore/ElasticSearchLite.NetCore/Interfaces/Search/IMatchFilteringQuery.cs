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
        IFilteredSearchQuery<TPoco> ByUsingOperator(ElasticOperators op = null);
        /// <summary>
        /// Fuzziness allows fuzzy matching based on the type of field being queried. 
        /// </summary>
        /// <param name="fuzziness">Level of fuzziness</param>
        /// <returns></returns>
        IFilteredSearchQuery<TPoco> WithFuzziness(int fuzziness);
    }
}
