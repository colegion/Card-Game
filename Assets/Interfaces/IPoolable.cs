using Helpers;

namespace Interfaces
{
    public interface IPoolable
    {
        public void OnPooled();
        public void OnFetchFromPool();
        public void OnReturnPool();
        public PoolableTypes GetPoolableType();
    }
}
