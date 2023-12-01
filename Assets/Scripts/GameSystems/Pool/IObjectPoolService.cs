using GameSystem.Core;

namespace GameSystem.Pool
{
    public interface IObjectPoolService
    {
        IPoolable Get();
        void Release(IPoolable item);
    }
}