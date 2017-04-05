using System;
using System.Collections.Generic;
using Elasticsearch.Net;
using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.NetCore.Queries.Generator;

namespace ElasticSearchLite.NetCore.Interfaces
{
    public class ElasticLiteClient : IDisposable
    {
        private bool disposedValue = false;
        private IStatementGenerator Generator { get; }
        public ElasticLowLevelClient Client { get; private set; }

        public ElasticLiteClient(string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentNullException(nameof(uri));
            }

            Client = new ElasticLowLevelClient(new ConnectionConfiguration(new Uri(uri)));
            Generator = new StatementGenerator();
        }

        public IEnumerable<T> ExecuteSearch<T>(SearchQuery<T> query) where T : IElasticPoco
        {
            var statement = Generator.Generate(query);
            var response = Client.Search<IEnumerable<T>>(new PostData<object>(statement));

            if (response.Success)
            {
                return response.Body;
            }

            throw response.OriginalException;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }
                
                Client = null;
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
