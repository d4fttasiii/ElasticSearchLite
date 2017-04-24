namespace ElasticSearchLite.NetCore.Interfaces
{
    /// <summary>
    /// A Search request containing a Term, Match or Range condition
    /// </summary>
    /// <typeparam name="TPoco">Poco type implementing the IElasticPoco interface</typeparam>
    public interface ISearchExecutable<TPoco> : IQuery where TPoco : IElasticPoco
    {
        /// <summary>
        /// Skips a certain number of documents from the result set. 
        /// </summary>
        /// <param name="skip">Number of documents to skip.</param>
        /// <returns></returns>
        ISearchExecutable<TPoco> Skip(int skip);
        /// <summary>
        /// Limits the number of documents a the results set.
        /// </summary>
        /// <param name="take">Number of documents in response.</param>
        /// <returns></returns>
        ISearchExecutable<TPoco> Take(int take);
    }
}
