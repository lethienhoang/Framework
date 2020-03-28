using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CosmosDb.DBContext
{
    public interface ICommosDbContext
    {
        Task<T> GetAsync<T>(string id, string databaseName, string collectionName) where T : class;

        Task<T> GetAsync<T>(string id, string partitionKey, string databaseName, string collectionName) where T : class;

        Task<Document> GetDocumentAsync(string id, string partitionKey, string databaseName, string collectionName);

        Task<IEnumerable<T>> GetAsync<T>(string databaseName, string collectionName) where T : class;

        Task<IEnumerable<T>> GetAsync<T>(Expression<Func<T, bool>> predicate, string databaseName, string collectionName) where T : class;

        Task<IEnumerable<T>> SearchAsync<T>(Expression<Func<T, bool>> predicate, string databaseName, string collectionName, int page = 1, int resultPerPage = 10) where T : class;

        Task<IEnumerable<T>> AddDocumentQuery<T>(string query, FeedOptions options, string databaseName, string collectionName) where T : class;

        Task<Document> AddAsync<T>(T item, string databaseName, string collectionName) where T : class;

        Task<Document> Addsync<T>(T item, RequestOptions options, string databaseName, string collectionName) where T : class;

        Task<Document> UpdateAsync<T>(string id, T item, string databaseName, string collectionName) where T : class;

        Task<ResourceResponse<Attachment>> AddAttachmentAsync(string attachmentsLink, object attachment, RequestOptions options, string databaseName, string collectionName);

        Task<ResourceResponse<Attachment>> GetAttachmentAsync(string attachmentLink, string partitionkey, string databaseName, string collectionName);

        Task<ResourceResponse<Attachment>> UpdateAttachmentAsync(Attachment attachment, RequestOptions options, string databaseName, string collectionName);

        Task DeleteAsync(string id, string databaseName, string collectionName);

        Task DeleteAsync(string id, string partitionKey, string databaseName, string collectionName);

        Task InitAsync(string databaseName, string collectionName);
    }
}
