namespace ElasticSearchLite.NetCore.Interfaces.Search
{
    public interface ILimitedResultSearchQuery<TPoco> : IExecutableSearchQuery<TPoco> where TPoco : IElasticPoco
    {
        /// <summary>
        /// Skips a certain number of documents.
        /// </summary>
        /// <param name="skip">Number of documents to skip (Offset).</param>
        /// <returns></returns>
        IExecutableSearchQuery<TPoco> Skip(int skip);
    }
}
