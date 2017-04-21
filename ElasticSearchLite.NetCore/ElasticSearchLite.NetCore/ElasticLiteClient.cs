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
        public ElasticLowLevelClient LowLevelClient { get; private set; }

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

            LowLevelClient = new ElasticLowLevelClient(settings);
        }
        /// <summary>
        /// Executes a SearchQuery using the Search API and returns a list of generic pocos.
        /// https://www.elastic.co/guide/en/elasticsearch/reference/5.3/_the_search_api.html
        /// </summary>
        /// <typeparam name="TPoco">Has to Implement the IElasticPoco interface</typeparam>
        /// <param name="query">SearchQuery object.</param>
        /// <returns>IEnumerable<TPoco></returns>
        public IEnumerable<TPoco> ExecuteSearch<TPoco>(IExecutableSearch<TPoco> executableSearchQuery) where TPoco : IElasticPoco
        {
            try
            {
                var query = executableSearchQuery as Search<TPoco>;
                var statement = Generator.Generate(query);
                var response = LowLevelClient.Search<string>(query.IndexName, query.TypeName, statement);

                if (!response.Success)
                {
                    throw response.OriginalException ?? new Exception($"Unsuccessful Elastic Request: {response.DebugInformation}");
                }

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
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Executes an IndexQuery using the Index API which creates a new document in the index.
        /// https://www.elastic.co/guide/en/elasticsearch/reference/5.3/docs-index_.html
        /// </summary>
        /// <typeparam name="TPoco">Has to Implement the IElasticPoco interface</typeparam>
        /// <param name="query">IndexQuery object</param>
        public void ExecuteIndex<TPoco>(Index<TPoco> query) where TPoco : IElasticPoco
        {
            try
            {
                var statement = Generator.Generate(query);

                var response = !string.IsNullOrEmpty(query.Poco.Id) ?
                    LowLevelClient.Index<string>(query.IndexName, query.TypeName, query.Poco.Id, statement) :
                    LowLevelClient.Index<string>(query.IndexName, query.TypeName, statement);

                if (!response.Success)
                {
                    throw response.OriginalException ?? new Exception($"Unsuccessful Elastic Request: {response.DebugInformation}");
                }

                var data = JObject.Parse(response.Body);
                query.Poco.Id = data[ElasticFields.Id.Name].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
        /// <summary>
        /// Executes an UpdateQuery using the Update API and updates a document identified by the Id.
        /// https://www.elastic.co/guide/en/elasticsearch/reference/5.3/docs-update.html
        /// </summary>
        /// <typeparam name="TPoco">Has to Implement the IElasticPoco interface</typeparam>
        /// <param name="query">UpdateQuery object</param>
        public void ExecuteUpdate<TPoco>(Update<TPoco> query) where TPoco : IElasticPoco
        {
            try
            {
                var statement = Generator.Generate(query);
                var response = LowLevelClient.Update<string>(query.IndexName, query.TypeName, query.Poco.Id, statement);

                if (!response.Success)
                {
                    throw response.OriginalException ?? new Exception($"Unsuccessful Elastic Request: {response.DebugInformation}");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
        /// <summary>
        /// Executes a DeleteQuery using the Delete API and removes a document from the associated index.
        /// https://www.elastic.co/guide/en/elasticsearch/reference/5.3/docs-delete.html
        /// </summary>
        /// <typeparam name="TPoco">Has to Implement the IElasticPoco interface</typeparam>
        /// <param name="query">DeleteQuery object</param>
        /// <returns>Returnes the number of effected documents.</returns>
        public int ExecuteDelete<TPoco>(Delete<TPoco> query) where TPoco : IElasticPoco
        {
            var statement = Generator.Generate(query);

            try
            {
                var response = !string.IsNullOrEmpty(query?.Poco.Id) ?
                    LowLevelClient.Delete<string>(query.IndexName, query.TypeName, query.Poco.Id) :
                    LowLevelClient.DeleteByQuery<string>(query.IndexName, statement);

                if (!response.Success)
                {
                    throw response.OriginalException ?? new Exception($"Unsuccessful Elastic Request: {response.DebugInformation}");
                }

                if (response.HttpMethod == HttpMethod.DELETE)
                {
                    return 1;
                }

                var data = JObject.Parse(response.Body);

                return data[ElasticFields.Total.Name].ToObject<int>();                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Executes the drop index using the Delete Index API and deletes the index.
        /// https://www.elastic.co/guide/en/elasticsearch/reference/5.3/indices-delete-index.html
        /// </summary>
        /// <typeparam name="TPoco">Has to Implement the IElasticPoco interface</typeparam>
        /// <param name="query">DropQuery object</param>
        public void ExecuteDrop(Drop query)
        {
            try
            {
                var response = LowLevelClient.IndicesDelete<string>(query.IndexName);

                if (!response.Success)
                {
                    throw response.OriginalException ?? new Exception($"Unsuccessful Elastic Request: {response.DebugInformation}");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TPoco">Has to Implement the IElasticPoco interface</typeparam>
        /// <param name="bulkQuery"></param>
        /// <returns></returns>
        public void ExecuteBulk<TPoco>(Bulk<TPoco> bulkQuery) where TPoco : IElasticPoco
        {
            try
            {
                var statement = Generator.Generate(bulkQuery);
                var response = LowLevelClient.Bulk<string>(bulkQuery.IndexName, statement);

                if (!response.Success)
                {
                    throw response.OriginalException ?? new Exception($"Unsuccessful Elastic Request: {response.DebugInformation}");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                LowLevelClient = null;
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
