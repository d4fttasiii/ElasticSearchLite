using ElasticSearchLite.NetCore.Models;
using System.Collections.Generic;

namespace ElasticSearchLite.NetCore.Queries
{
    public class Create
    {
        public static CreateQuery Index(string indexName) => new CreateQuery(indexName);

        public sealed class CreateQuery : AbstractBaseQuery
        {
            internal Dictionary<string, ElasticField> Mapping { get; }

            internal CreateQuery(string indexName) : base(indexName)
            {
                Mapping = new Dictionary<string, ElasticField>();
            }

        }
    }
}
