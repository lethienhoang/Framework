namespace Framework.Caching
{
    public interface ICache
    {
        void Set<T>(object key, T value);

        T Get<T>(object key);
    }
}
