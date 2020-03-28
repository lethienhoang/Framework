using Framework.Types;
using System.Threading.Tasks;

namespace Framework.CQRS.Dispatchers
{
    public interface IQueryDispatcher
    {
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
    }
}
