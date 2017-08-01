using ElasticSearchLite.NetCore.Interfaces.BoolHighlight;

namespace ElasticSearchLite.NetCore.Interfaces.Highlight
{
    public interface IHighlightQueryExecutable<TPoco> : 
        IBaseBoolHighlightQueryExecutable<
            TPoco, 
            IHighlightQueryShouldAdded<TPoco>, 
            IHighlightQueryMustAdded<TPoco>,
            IHighlightQueryMustNotAdded<TPoco>, 
            IHighlightQueryFilterAdded<TPoco>>,
        IQuery 
        where TPoco : IElasticPoco
    {
        /// <summary>
        /// Sets minimum_should_match parameter.
        /// </summary>
        /// <param name="minimumNumber">Bigger than 0</param>
        /// <returns></returns>
        IHighlightQueryExecutable<TPoco> ShouldMatchAtLeast(int minimumNumber);
        /// <summary>
        /// Sets fragment_size parameter.
        /// </summary>
        /// <param name="fragmentSize">Bigger than 0</param>
        /// <returns></returns>
        IHighlightQueryExecutable<TPoco> LimitFragmentSizeTo(int fragmentSize);
        /// <summary>
        /// Sets number_of_fragments parameter.
        /// </summary>
        /// <param name="numberOfFragments">Bigger than 0</param>
        /// <returns></returns>
        IHighlightQueryExecutable<TPoco> LimitFragmentsTo(int numberOfFragments);
        /// <summary>
        /// Limits the size of the document result set.
        /// </summary>
        /// <param name="take"></param>
        /// <returns></returns>
        IHighlightQueryExecutable<TPoco> Take(int take);
        /// <summary>
        /// Skips a certain number of documents.
        /// </summary>
        /// <param name="skip">Number of documents to skip (Offset).</param>
        /// <returns></returns>
        IHighlightQueryExecutable<TPoco> Skip(int skip);
    }
}
