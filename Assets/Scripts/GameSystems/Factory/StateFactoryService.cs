using System;
using System.Collections.Generic;
using GameSystem.Pool;
using GameSystem.StateManger;
using IPoolable = GameSystem.Core.IPoolable;

namespace GameSystem.Factory
{
    public interface IStateFactoryService
    {
        void RegisterCharacterState<T>() where T : CharacterState, IPoolable, new();
        T CreateCharacterState<T>() where T : CharacterState, IPoolable, new();
        void ReleaseState<T>(T state) where T : CharacterState, IPoolable;
    }

    public class StateFactoryServiceService : IStateFactoryService
    {
        private Dictionary<Type, IObjectPoolService> _pools = new Dictionary<Type, IObjectPoolService>();

        public void RegisterCharacterState<T>() where T : CharacterState, IPoolable, new()
        {
            Type type = typeof(T);
            if (!_pools.ContainsKey(type))
            {
                _pools[type] = new ObjectPoolService<T>();
            }
        }

        public T CreateCharacterState<T>() where T : CharacterState, IPoolable, new()
        {
            Type type = typeof(T);
            if (!_pools.TryGetValue(type, out IObjectPoolService pool))
            {
                // If no pool is registered, register one and then proceed
                RegisterCharacterState<T>();
                pool = _pools[type];
            }

            return (T) pool.Get();
        }

        public void ReleaseState<T>(T state) where T : CharacterState, IPoolable
        {
            Type type = typeof(T);
            if (!_pools.TryGetValue(type, out IObjectPoolService pool))
            {
                throw new InvalidOperationException($"No pool registered for type {type}.");
            }

            pool.Release(state);
        }
    }
}