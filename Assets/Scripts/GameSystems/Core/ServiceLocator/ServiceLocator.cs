using System;
using System.Collections.Generic;

namespace GameSystem.Core.ServiceLocator
{
    public class ServiceLocator
    {
        private static ServiceLocator _instance;
        private Dictionary<Type, object> _services;

        // Private constructor
        private ServiceLocator()
        {
            _services = new Dictionary<Type, object>();
        }


        public static ServiceLocator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ServiceLocator();
                }

                return _instance;
            }
        }


        public void Register<TInterface, TService>(TService service)
            where TInterface : class
            where TService : class, TInterface
        {
            _services[typeof(TInterface)] = service;
        }


        public TInterface Get<TInterface>() where TInterface : class
        {
            var serviceType = typeof(TInterface);
            if (_services.TryGetValue(serviceType, out var service))
            {
                return service as TInterface;
            }


            throw new InvalidOperationException($"Service of type {serviceType.Name} not registered.");
        }
    }
}