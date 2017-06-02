namespace ElasticSearchLite.NetCore.Interfaces.Search
{
    public interface IMatchPhraseFilteringQuery<TPoco> : IFilteredSearchQuery<TPoco> where TPoco : IElasticPoco
    {
        /// <summary>
        /// Adding flexibility to the match_phrase query with slop.
        /// The slop parameter tells how far apart terms are allowed to be while still considering the document a match
        /// </summary>
        /// <param name="slop"></param>
        /// <returns></returns>
        IFilteredSearchQuery<TPoco> Slop(int slop);
    }
}
