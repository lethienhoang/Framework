using System.Threading.Tasks;

namespace Framework
{
    public interface IInitializer
    {
        Task InitializeAsync();
    }
}
