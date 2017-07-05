using Elasticsearch.Net;
using ElasticSearchLite.NetCore.Exceptions;
using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Interfaces.Bool;
using ElasticSearchLite.NetCore.Interfaces.Search;
using ElasticSearchLite.NetCore.Interfaces.Highlight;
using ElasticSearchLite.NetCore.Models;
using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.NetCore.Queries.Generator;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ElasticSearchLite.NetCore.Queries.Bool;
using static ElasticSearchLite.NetCore.Queries.Delete;
using static ElasticSearchLite.NetCore.Queries.Get;
using static ElasticSearchLite.NetCore.Queries.Search;
using static ElasticSearchLite.NetCore.Queries.Highlight;

namespace ElasticSearchLite.NetCore
{
    // TODO: Handling different http status codes!
    public class ElasticLiteClient : IDisposable
    {
        private bool disposedValue = false;
        private IStatementFactory Generator { get; } = new StatementFactory();
        public ElasticLowLevelClient LowLevelClient { get; private set; }
        public NamingStrategy NameingStrategy { get { return Generator.NamingStrategy; } set { Generator.NamingStrategy = value; } }

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
        /// Executes a  get API call to get a typed JSON document from the index based on its id.
        /// https://www.elastic.co/guide/en/elasticsearch/reference/5.4/docs-get.html
        /// </summary>
        /// <typeparam name="TPoco"></typeparam>
        /// <param name="getQuery"></param>
        /// <returns></returns>
        public TPoco ExecuteGet<TPoco>(GetQuery<TPoco> getQuery)
            where TPoco : IElasticPoco
        {
            IndexExists(getQuery.IndexName);
            var response = LowLevelClient.Get<string>(getQuery.IndexName, getQuery.TypeName, getQuery.Id.ToString());
            if (!response.Success)
            {
                throw response.OriginalException ?? new Exception($"Unsuccessful Elastic Request: {response.DebugInformation}");
            }
            var data = JObject.Parse(response.Body)?.First;

            return MapResponseToPoco<TPoco>(data);
        }

        /// <summary>
        /// Executes a  get API call to get a typed JSON document from the index based on its id.
        /// https://www.elastic.co/guide/en/elasticsearch/reference/5.4/docs-get.html
        /// </summary>
        /// <typeparam name="TPoco"></typeparam>
        /// <param name="getQuery"></param>
        /// <returns></returns>
        public async Task<TPoco> ExecuteGetAsync<TPoco>(GetQuery<TPoco> getQuery)
            where TPoco : IElasticPoco
        {
            IndexExists(getQuery.IndexName);
            var response = await LowLevelClient.GetAsync<string>(getQuery.IndexName, getQuery.TypeName, getQuery.Id.ToString());
            if (!response.Success)
            {
                throw response.OriginalException ?? new Exception($"Unsuccessful Elastic Request: {response.DebugInformation}");
            }
            var data = JObject.Parse(response.Body)?.First;

            return MapResponseToPoco<TPoco>(data);
        }
        /// <summary>
        /// Executes a SearchQuery using the Search API and returns a list of generic pocos.
        /// https://www.elastic.co/guide/en/elasticsearch/reference/5.4/_the_search_api.html
        /// </summary>
        /// <typeparam name="TPoco">Has to Implement the IElasticPoco interface</typeparam>
        /// <param name="searchQuery">SearchQuery object.</param>
        /// <returns>IEnumerable<TPoco></returns>
        public IEnumerable<TPoco> ExecuteSearch<TPoco>(IExecutableSearchQuery<TPoco> searchQuery)
            where TPoco : IElasticPoco
        {
            var query = searchQuery as SearchQuery<TPoco>;
            IndexExists(query.IndexName);

            return ProcessSeachResponse<TPoco>(LowLevelClient.Search<string>(query.IndexName, Generator.Generate(query)));
        }
        /// <summary>
        /// Executes an asyncronus SearchQuery using the Search API and returns a list of generic pocos.
        /// https://www.elastic.co/guide/en/elasticsearch/reference/5.4/_the_search_api.html
        /// </summary>
        /// <typeparam name="TPoco">Has to Implement the IElasticPoco interface</typeparam>
        /// <param name="searchQuery">SearchQuery object.</param>
        /// <returns>IEnumerable<TPoco></returns>
        public async Task<IEnumerable<TPoco>> ExecuteSearchAsync<TPoco>(IExecutableSearchQuery<TPoco> searchQuery)
             where TPoco : IElasticPoco
        {
            var query = searchQuery as SearchQuery<TPoco>;
            IndexExists(query.IndexName);

            return ProcessSeachResponse<TPoco>(await LowLevelClient.SearchAsync<string>(query.IndexName, Generator.Generate(query)));
        }
        /// <summary>
        /// A query that matches documents matching boolean combinations of other queries. The bool query maps to Lucene BooleanQuery. 
        /// It is built using one or more boolean clauses, each clause with a typed occurrence.
        /// https://www.elastic.co/guide/en/elasticsearch/reference/5.4/query-dsl-bool-query.html
        /// </summary>
        /// <param name="boolQuery"></param>
        /// <returns>IEnumerable<TPoco></returns>
        public IEnumerable<TPoco> ExecuteBool<TPoco>(IBoolQueryExecutable<TPoco> boolQuery) where TPoco : IElasticPoco
        {
            var query = boolQuery as BoolQuery<TPoco>;
            var response = LowLevelClient.Search<string>(query.IndexName, Generator.Generate(query));

            return ProcessSeachResponse<TPoco>(response);
        }
        /// <summary>
        /// A query that matches documents matching boolean combinations of other queries. The bool query maps to Lucene BooleanQuery. 
        /// It is built using one or more boolean clauses, each clause with a typed occurrence.
        /// https://www.elastic.co/guide/en/elasticsearch/reference/5.4/query-dsl-bool-query.html
        /// </summary>
        /// <param name="boolQuery"></param>
        /// <returns>IEnumerable<TPoco></returns>
        public async Task<IEnumerable<TPoco>> ExecuteBoolAsync<TPoco>(IBoolQueryExecutable<TPoco> boolQuery) where TPoco : IElasticPoco
        {
            var query = boolQuery as BoolQuery<TPoco>;
            var response = await LowLevelClient.SearchAsync<string>(query.IndexName, Generator.Generate(query));

            return ProcessSeachResponse<TPoco>(response);
        }
        /// <summary>
        /// A query that matches documents matching boolean combinations of other queries. The bool query maps to Lucene BooleanQuery. 
        /// It is built using one or more boolean clauses, each clause with a typed occurrence.
        /// https://www.elastic.co/guide/en/elasticsearch/reference/5.4/search-request-highlighting.html#_highlight_query
        /// </summary>
        /// <param name="boolQuery"></param>
        /// <returns>IEnumerable<TPoco></returns>
        public IEnumerable<ElasticHighlightResponse> ExecuteHighLight<TPoco>(IHighlightQueryExecutable<TPoco> highlightQuery) where TPoco : IElasticPoco
        {
            var query = highlightQuery as HighlightQuery<TPoco>;
            var response = LowLevelClient.Search<string>(query.IndexName, Generator.Generate(query));

            return ProcessHighLightResponse<TPoco>(response);
        }
        /// <summary>
        /// A query that matches documents matching boolean combinations of other queries. The bool query maps to Lucene BooleanQuery. 
        /// It is built using one or more boolean clauses, each clause with a typed occurrence.
        /// https://www.elastic.co/guide/en/elasticsearch/reference/5.4/search-request-highlighting.html#_highlight_query
        /// </summary>
        /// <param name="boolQuery"></param>
        /// <returns>IEnumerable<TPoco></returns>
        public async Task<IEnumerable<ElasticHighlightResponse>> ExecuteHighLightAsync<TPoco>(IHighlightQueryExecutable<TPoco> highlightQuery) where TPoco : IElasticPoco
        {
            var query = highlightQuery as HighlightQuery<TPoco>;
            var response = await LowLevelClient.SearchAsync<string>(query.IndexName, Generator.Generate(query));

            return ProcessHighLightResponse<TPoco>(response);
        }
        /// <summary>
        /// Executes an IndexQuery using the Index API which creates a new document in the index.
        /// https://www.elastic.co/guide/en/elasticsearch/reference/5.4/docs-index_.html
        /// </summary>
        /// <param name="query">IndexQuery object</param>
        public void ExecuteIndex(Index query)
        {
            var statement = Generator.Generate(query);
            var response = !string.IsNullOrEmpty(query.Poco.Id) ?
                LowLevelClient.Index<string>(query.IndexName, query.TypeName, query.Poco.Id, statement) :
                LowLevelClient.Index<string>(query.IndexName, query.TypeName, statement);

            ProcessIndexResponse(query, response);
        }
        /// <summary>
        /// Executes an asyncronus IndexQuery using the Index API which creates a new document in the index.
        /// https://www.elastic.co/guide/en/elasticsearch/reference/5.4/docs-index_.html
        /// </summary>
        /// <param name="query">IndexQuery object</param>
        public async Task ExecuteIndexAsync(Index query)
        {
            var statement = Generator.Generate(query);
            var response = !string.IsNullOrEmpty(query.Poco.Id) ?
                LowLevelClient.IndexAsync<string>(query.IndexName, query.TypeName, query.Poco.Id, statement) :
                LowLevelClient.IndexAsync<string>(query.IndexName, query.TypeName, statement);

            ProcessIndexResponse(query, await response);
        }
        /// <summary>
        /// Executes an UpdateQuery using the Update API and updates a document identified by the Id.
        /// https://www.elastic.co/guide/en/elasticsearch/reference/5.4/docs-update.html
        /// </summary>
        /// <typeparam name="TPoco">Has to Implement the IElasticPoco interface</typeparam>
        /// <param name="query">UpdateQuery object</param>
        public void ExecuteUpdate(Update query)
        {
            var statement = Generator.Generate(query);
            var response = LowLevelClient.Update<string>(query.IndexName, query.TypeName, query.Poco.Id, statement);

            if (!response.Success)
            {
                throw response.OriginalException ?? new Exception($"Unsuccessful Elastic Request: {response.DebugInformation}");
            }
        }
        /// <summary>
        /// Executes an asyncronus UpdateQuery using the Update API and updates a document identified by the Id.
        /// https://www.elastic.co/guide/en/elasticsearch/reference/5.4/docs-update.html
        /// </summary>
        /// <typeparam name="TPoco">Has to Implement the IElasticPoco interface</typeparam>
        /// <param name="query">UpdateQuery object</param>
        public async Task ExecuteUpdateAsync(Update query)
        {
            var statement = Generator.Generate(query);
            var response = await LowLevelClient.UpdateAsync<string>(query.IndexName, query.TypeName, query.Poco.Id, statement);

            if (!response.Success)
            {
                throw response.OriginalException ?? new Exception($"Unsuccessful Elastic Request: {response.DebugInformation}");
            }
        }
        /// <summary>
        /// Executes a DeleteQuery using the Delete API and removes a document from the associated index.
        /// https://www.elastic.co/guide/en/elasticsearch/reference/5.4/docs-delete.html
        /// </summary>
        /// <typeparam name="TPoco">Has to Implement the IElasticPoco interface</typeparam>
        /// <param name="query">DeleteQuery object</param>
        /// <returns>Returnes the number of effected documents.</returns>
        public int ExecuteDelete<TPoco>(IDeleteExecutable<TPoco> deleteQuery)
            where TPoco : IElasticPoco
        {
            var query = deleteQuery as DeleteQuery<TPoco>;
            if (query == null)
            {
                throw new Exception("Invalid delete request!");
            }

            var statement = Generator.Generate(query);
            var response = !string.IsNullOrEmpty(query?.Poco.Id) ?
                LowLevelClient.Delete<string>(query.IndexName, query.TypeName, query.Poco.Id) :
                LowLevelClient.DeleteByQuery<string>(query.IndexName, statement);

            return ProcessDeleteResponse(response);
        }
        /// <summary>
        /// Executes an asyncronus DeleteQuery using the Delete API and removes a document from the associated index.
        /// https://www.elastic.co/guide/en/elasticsearch/reference/5.4/docs-delete.html
        /// </summary>
        /// <typeparam name="TPoco">Has to Implement the IElasticPoco interface</typeparam>
        /// <param name="query">DeleteQuery object</param>
        /// <returns>Returnes the number of effected documents.</returns>
        public async Task<int> ExecuteDeleteAsync<TPoco>(IDeleteExecutable<TPoco> deleteQuery)
            where TPoco : IElasticPoco
        {
            var query = deleteQuery as DeleteQuery<TPoco>;
            if (query == null)
            {
                throw new Exception("Invalid delete request!");
            }

            var statement = Generator.Generate(query);
            var response = !string.IsNullOrEmpty(query?.Poco.Id) ?
                LowLevelClient.DeleteAsync<string>(query.IndexName, query.TypeName, query.Poco.Id) :
                LowLevelClient.DeleteByQueryAsync<string>(query.IndexName, statement);

            return ProcessDeleteResponse(await response);
        }
        /// <summary>
        /// Drops an index using the Delete Index API.
        /// https://www.elastic.co/guide/en/elasticsearch/reference/5.4/indices-delete-index.html
        /// </summary>
        /// <typeparam name="TPoco">Has to Implement the IElasticPoco interface</typeparam>
        /// <param name="query">DropQuery object</param>
        public void ExecuteDrop(Drop query)
        {
            IndexExists(query.IndexName);

            var response = LowLevelClient.IndicesDelete<string>(query.IndexName);

            if (!response.Success)
            {
                throw response.OriginalException ?? new Exception($"Unsuccessful Elastic Request: {response.DebugInformation}");
            }
        }
        /// <summary>
        /// Drops an index using by call the asyncronus Delete Index API.
        /// https://www.elastic.co/guide/en/elasticsearch/reference/5.4/indices-delete-index.html
        /// </summary>
        /// <typeparam name="TPoco">Has to Implement the IElasticPoco interface</typeparam>
        /// <param name="query">DropQuery object</param>
        public async Task ExecuteDropAsync(Drop query)
        {
            var response = await LowLevelClient.IndicesDeleteAsync<string>(query.IndexName);

            if (!response.Success)
            {
                throw response.OriginalException ?? new Exception($"Unsuccessful Elastic Request: {response.DebugInformation}");
            }
        }
        /// <summary>
        /// Executes a bulk statement using the Bulk API
        /// https://www.elastic.co/guide/en/elasticsearch/reference/current/docs-bulk.html
        /// </summary>
        /// <param name="bulkQuery"></param>
        /// <returns></returns>
        public void ExecuteBulk(Bulk bulkQuery)
        {
            var statement = Generator.Generate(bulkQuery);
            var response = LowLevelClient.Bulk<string>(bulkQuery.IndexName, statement);

            if (!response.Success)
            {
                throw response.OriginalException ?? new Exception($"Unsuccessful Elastic Request: {response.DebugInformation}");
            }
        }
        /// <summary>
        /// Executes a bulk statement using the asyncronus Bulk API
        /// https://www.elastic.co/guide/en/elasticsearch/reference/current/docs-bulk.html
        /// </summary>
        /// <param name="bulkQuery"></param>
        /// <returns></returns>
        public async Task ExecuteBulkAsync(Bulk bulkQuery)
        {
            var statement = Generator.Generate(bulkQuery);
            var response = await LowLevelClient.BulkAsync<string>(bulkQuery.IndexName, statement);

            if (!response.Success)
            {
                throw response.OriginalException ?? new Exception($"Unsuccessful Elastic Request: {response.DebugInformation}");
            }
        }
        private void IndexExists(string indexName)
        {
            if (LowLevelClient.IndicesExists<string>(indexName).HttpStatusCode == 404)
            {
                throw new IndexNotAvailableException($"{indexName} index doesn't exist");
            }
        }

        private static IEnumerable<ElasticHighlightResponse> ProcessHighLightResponse<TPoco>(ElasticsearchResponse<string> response) where TPoco : IElasticPoco
        {
            if (!response.Success)
            {
                throw response.OriginalException ?? new Exception($"Unsuccessful Elastic Request: {response.DebugInformation}");
            }

            var data = JObject.Parse(response.Body);
            var hits = new List<ElasticHighlightResponse>();
            var total = data[ElasticFields.Hits.Name][ElasticFields.Total.Name].ToObject<long>();

            foreach (var jToken in data[ElasticFields.Hits.Name][ElasticFields.Hits.Name])
            {
                var hit = new ElasticHighlightResponse
                {
                    Id = jToken[ElasticFields.Id.Name].ToString(),
                    Index = jToken[ElasticFields.Index.Name].ToString(),
                    Type = jToken[ElasticFields.Type.Name].ToString(),
                    Score = jToken[ElasticFields.Score.Name].ToObject<double?>(),
                    Total = total,
                    Highlight = jToken[ElasticFields.Highlight.Name].ToObject<Dictionary<string, string[]>>(),
                };

                hits.Add(hit);
            }

            return hits;
        }

        private static IEnumerable<TPoco> ProcessSeachResponse<TPoco>(ElasticsearchResponse<string> response) where TPoco : IElasticPoco
        {
            if (!response.Success)
            {
                throw response.OriginalException ?? new Exception($"Unsuccessful Elastic Request: {response.DebugInformation}");
            }

            var data = JObject.Parse(response.Body);
            var hits = new List<TPoco>();
            var total = data[ElasticFields.Hits.Name][ElasticFields.Total.Name].ToObject<long>();

            foreach (var jToken in data[ElasticFields.Hits.Name][ElasticFields.Hits.Name])
            {
                var hit = MapResponseToPoco<TPoco>(jToken);
                hit.Total = total;
                hits.Add(hit);
            }

            return hits;
        }

        private static TPoco MapResponseToPoco<TPoco>(JToken jToken) where TPoco : IElasticPoco
        {
            var document = jToken[ElasticFields.Source.Name].ToObject<TPoco>();
            document.Id = jToken[ElasticFields.Id.Name].ToString();
            document.Index = jToken[ElasticFields.Index.Name].ToString();
            document.Type = jToken[ElasticFields.Type.Name].ToString();
            document.Score = jToken[ElasticFields.Score.Name].ToObject<double?>();

            return document;
        }

        private static void ProcessIndexResponse(Index query, ElasticsearchResponse<string> response)
        {
            if (!response.Success)
            {
                throw response.OriginalException ?? new Exception($"Unsuccessful Elastic Request: {response.DebugInformation}");
            }

            var data = JObject.Parse(response.Body);
            query.Poco.Id = data[ElasticFields.Id.Name].ToString();
        }

        private static int ProcessDeleteResponse(ElasticsearchResponse<string> response)
        {
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
