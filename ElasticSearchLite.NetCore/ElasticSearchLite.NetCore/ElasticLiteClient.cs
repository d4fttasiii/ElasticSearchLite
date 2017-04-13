using System;
using System.Collections.Generic;
using Elasticsearch.Net;
using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.NetCore.Queries.Generator;
using ElasticSearchLite.NetCore.Interfaces;
using System.Linq;
using ElasticSearchLite.NetCore.Models;
using Newtonsoft.Json.Linq;

namespace ElasticSearchLite.NetCore
{
    public class ElasticLiteClient : IDisposable
    {
        private bool disposedValue = false;
        private IStatementFactory Generator { get; } = new StatementFactory();
        public ElasticLowLevelClient Client { get; private set; }

        /// <summary>
        /// Creates ElasticSearchLite Client which uses the low level elastic client  
        /// </summary>
        /// <param name="uris">Connection URIs as string</param>
        public ElasticLiteClient(params string[] uris)
        {
            if (uris == null)
            {
                throw new ArgumentNullException(nameof(uris));
            }
            var uriObjects = uris.Select(u => new Uri(u));
            BuildUpLowLevelConnection(uriObjects);
        }

        /// <summary>
        /// Creates ElasticSearchLite Client which uses the low level elastic client  
        /// </summary>
        /// <param name="uris">Connection URIs</param>
        public ElasticLiteClient(params Uri[] uris)
        {
            if (uris == null)
            {
                throw new ArgumentNullException(nameof(uris));
            }
            BuildUpLowLevelConnection(uris);
        }

        private void BuildUpLowLevelConnection(IEnumerable<Uri> uris)
        {
            if (!uris.Any())
            {
                return;
            }

            var connectionPool = new StickyConnectionPool(uris);
            var settings = new ConnectionConfiguration(connectionPool).ThrowExceptions().DisableDirectStreaming();

            Client = new ElasticLowLevelClient(settings);
        }

        public IEnumerable<TPoco> ExecuteSearch<TPoco>(SearchQuery<TPoco> query) where TPoco : IElasticPoco
        {
            var statement = Generator.Generate(query);
            var response = Client.Search<string>(query.IndexName, query.TypeName, statement);

            if (response.Success)
            {
                var data = JObject.Parse(response.Body);
                var hits = new List<TPoco>();
                foreach (var x in data[ElasticFields.Hits.Name][ElasticFields.Hits.Name])
                {
                    var document = x[ElasticFields.Source.Name].ToObject<TPoco>();
                    document.Id = x[ElasticFields.Id.Name].ToString();
                    document.Index = x[ElasticFields.Index.Name].ToString();
                    document.Type = x[ElasticFields.Type.Name].ToString();
                    document.Score = x[ElasticFields.Score.Name].ToObject<double>();
                    hits.Add(document);
                }

                return hits;
            }

            throw response.OriginalException;
        }

        public void ExecuteIndex<TPoco>(IndexQuery<TPoco> query) where TPoco : IElasticPoco
        {
            var statement = Generator.Generate(query);
            var response = Client.Index<string>(query.IndexName, query.TypeName, statement);

            if (response.Success)
            {
                var data = JObject.Parse(response.Body);
                query.Poco.Id = data[ElasticFields.Id.Name].ToString();
                return;
            }

            throw response.OriginalException;
        }

        public bool ExecuteUpdate<TPoco>(UpdateQuery<TPoco> query) where TPoco : IElasticPoco
        {
            var statement = Generator.Generate(query);
            var response = Client.Update<string>(query.IndexName, query.TypeName, query.Poco.Id, statement);

            if (response.Success)
            {
                return true;
            }

            throw response.OriginalException;
        }

        public void ExecuteDelete<TPoco>(DeleteQuery<TPoco> query) where TPoco : IElasticPoco
        {
            var statement = Generator.Generate(query);
            var response = Client.DeleteByQuery<string>(query.IndexName, statement);

            if (response.Success)
            {
                var data = JObject.Parse(response.Body);
            }
        }

        public bool ExecuteDrop<TPoco>(DropQuery<TPoco> query) where TPoco : IElasticPoco
        {
            var response = Client.IndicesDelete<string>(query.IndexName);

            return response.Success;
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
