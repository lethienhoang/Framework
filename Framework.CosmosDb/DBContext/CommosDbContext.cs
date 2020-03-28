using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace Framework.CosmosDb.DBContext
{
    public class CommosDbContext : ICommosDbContext
    {
        private readonly IDocumentClient _client;
        private readonly ILogger<CommosDbContext> _logger;
        private DocumentCollection collection;
        public CommosDbContext(IDocumentClient client, ILogger<CommosDbContext> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<Document> AddAsync<T>(T item, string databaseName, string collectionName) where T : class
        {
            await InitAsync(databaseName, collectionName);

            return await _client.CreateDocumentAsync(
                CreateDocumentCollectionUri(databaseName, collectionName),
                item);
        }

        public async Task<ResourceResponse<Attachment>> AddAttachmentAsync(
            string attachmentsLink,
            object attachment,
            RequestOptions options,
            string databaseName,
            string collectionName)
        {
            await InitAsync(databaseName, collectionName);

            return await _client.CreateAttachmentAsync(attachmentsLink, attachment, options);
        }

        public async Task<IEnumerable<T>> AddDocumentQuery<T>(
            string query,
            FeedOptions options,
            string databaseName,
            string collectionName) where T : class
        {
            await InitAsync(databaseName, collectionName);

            return _client.CreateDocumentQuery<T>(collection.DocumentsLink, query, options).AsEnumerable();
        }

        public async Task<Document> Addsync<T>(
            T item,
            RequestOptions options,
            string databaseName,
            string collectionName) where T : class
        {
            await InitAsync(databaseName, collectionName);

            return await _client.CreateDocumentAsync(

                CreateDocumentCollectionUri(databaseName, collectionName),
                item,
                options);
        }

        public async Task DeleteAsync(string id, string databaseName, string collectionName)
        {
            await InitAsync(databaseName, collectionName);

            await _client.DeleteDocumentAsync(CreateDocumentUri(databaseName, collectionName, id));
        }

        public async Task DeleteAsync(string id, string partitionKey, string databaseName, string collectionName)
        {
            await InitAsync(databaseName, collectionName);

            await _client.DeleteDocumentAsync(
                CreateDocumentUri(databaseName, collectionName, id),
                new RequestOptions 
                { 
                     PartitionKey = new PartitionKey(partitionKey)
                });
        }

        public async Task<T> GetAsync<T>(string id, string databaseName, string collectionName) where T : class
        {
            await InitAsync(databaseName, collectionName);

            return await _client.ReadDocumentAsync<T>(CreateDocumentUri(databaseName, collectionName, id));
        }

        public async Task<T> GetAsync<T>(string id, string partitionKey, string databaseName, string collectionName) where T : class
        {
            await InitAsync(databaseName, collectionName);

            return await _client.ReadDocumentAsync<T>(
                CreateDocumentUri(databaseName, collectionName, id), 
                new RequestOptions 
                {
                    PartitionKey = new PartitionKey(partitionKey)
                });
        }

        public async Task<ResourceResponse<Attachment>> GetAttachmentAsync(
            string attachmentLink,
            string partitionkey,
            string databaseName,
            string collectionName)
        {
            await InitAsync(databaseName, collectionName);

            return await _client.ReadAttachmentAsync(
                attachmentLink,
                new RequestOptions 
                { 
                    PartitionKey = new PartitionKey(partitionkey)
                });
        }

        public async Task<Document> GetDocumentAsync(
            string id,
            string partitionKey,
            string databaseName,
            string collectionName)
        {
            await InitAsync(databaseName, collectionName);

            return await _client.ReadDocumentAsync(
                CreateDocumentUri(databaseName, collectionName, id),
                new RequestOptions
                {
                    PartitionKey = new PartitionKey(partitionKey)
                });
        }

        public async Task<IEnumerable<T>> GetAsync<T>(string databaseName, string collectionName) where T : class
        {
            await InitAsync(databaseName, collectionName);

            var query = _client.CreateDocumentQuery<T>(
                CreateDocumentCollectionUri(databaseName, collectionName),
                GetFeedOptions())
                .AsDocumentQuery();

            var results = new List<T>();

            do
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }
            while (query.HasMoreResults);

            return results;
        }

        public async Task<IEnumerable<T>> GetAsync<T>(
            Expression<Func<T, bool>> predicate,
            string databaseName,
            string collectionName) where T : class
        {
            await InitAsync(databaseName, collectionName);

            var query = _client.CreateDocumentQuery<T>(
                CreateDocumentCollectionUri(databaseName, collectionName),
                GetFeedOptions())
                .Where(predicate)
                .AsDocumentQuery();

            var results = new List<T>();

            do
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }
            while (query.HasMoreResults);

            return results;
        }

        public async Task<IEnumerable<T>> SearchAsync<T>(
            Expression<Func<T, bool>> predicate,
            string databaseName,
            string collectionName,
            int page = 1,
            int resultPerPage = 10) where T : class
        {
            await InitAsync(databaseName, collectionName);

            return _client.CreateDocumentQuery<T>(
                CreateDocumentCollectionUri(databaseName, collectionName),
                GetFeedOptions())
                .Where(predicate)
                .Paginate(page, resultPerPage);
        }

        public async Task<Document> UpdateAsync<T>(
            string id,
            T item,
            string databaseName,
            string collectionName) where T : class
        {
            await InitAsync(databaseName, collectionName);

            return await _client.ReplaceDocumentAsync(CreateDocumentUri(databaseName, collectionName, id), item);
        }

        public async Task<ResourceResponse<Attachment>> UpdateAttachmentAsync(
            Attachment attachment,
            RequestOptions options,
            string databaseName,
            string collectionName)
        {
            await InitAsync(databaseName, collectionName);

            return await _client.ReplaceAttachmentAsync(attachment, options);
        }

        public async Task InitAsync(string databaseName, string collectionName)
        {
            await GetDatabaseAsync(databaseName);
            await GetDocumentCollectionAsync(databaseName, collectionName);
        }

        #region Setting up

        private Uri CreateDocumentCollectionUri(string databaseName, string collectionName)
        {
            return UriFactory.CreateDocumentCollectionUri(databaseName, collectionName);
        }

        private Uri CreateDocumentUri(string databaseName, string collectionName, string id)
        {
            return UriFactory.CreateDocumentUri(databaseName, collectionName, id);
        }

        private FeedOptions GetFeedOptions()
        {
            return new FeedOptions
            {
                MaxItemCount = -1,
                EnableCrossPartitionQuery = true
            };
        }

        private async Task GetDatabaseAsync(string databaseName)
        {
            try
            {
                var dbUri = UriFactory.CreateDatabaseUri(databaseName);
                await _client.ReadDatabaseAsync(dbUri);
            }
            catch (DocumentClientException de)
            {
                if(de.StatusCode == HttpStatusCode.NotFound)
                {
                    await _client.CreateDatabaseAsync(new Database
                    {
                        Id = databaseName
                    });
                }
                else
                {
                    _logger.LogError(de.Message);
                    throw;
                }
            }

        }

        private async Task GetDocumentCollectionAsync(string databaseName, string collectionName)
        {
            try
            {
                await _client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName));
            }
            catch(DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    DocumentCollection collectionInfo = new DocumentCollection();
                    collectionInfo.Id = collectionName;

                    collectionInfo.IndexingPolicy = new IndexingPolicy(new RangeIndex(DataType.String) { Precision = -1 });

                    await _client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(databaseName),
                        new DocumentCollection { Id = collectionName },
                        new RequestOptions { OfferThroughput = 400 });
                }
                else
                {
                    _logger.LogError(de.Message);
                    throw;
                }
            }
        }
        #endregion
    }
}
