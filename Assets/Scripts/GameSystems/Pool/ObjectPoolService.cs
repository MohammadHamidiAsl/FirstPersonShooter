using System.Collections.Generic;
using GameSystem.Core;

namespace GameSystem.Pool
{
    public class ObjectPoolService<T> : IObjectPoolService where T : IPoolable, new()
    {
        private Queue<T> _pool = new Queue<T>();
        private const int MaxPoolSize = 7;

        public IPoolable Get()
        {
            return _pool.Count > 0 ? _pool.Dequeue() : new T();
        }

        public void Release(IPoolable item)
        {
            if (_pool.Count < MaxPoolSize && item is T tItem)
            {
                tItem.Reset();
                _pool.Enqueue(tItem);
            }
            
        }
    }
}