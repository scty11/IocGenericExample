using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyTestsIOC
{
    public class Container
    {
        //stores the mappings for the types we want.
        Dictionary<Type, Type> _map = new Dictionary<Type, Type>();
        //Creates the mapping process using a builder which is chained
        //to create the mappings.
        public ContainerBuilder For<T1>()
        {
            return For(typeof(T1));
        }
        public ContainerBuilder For(Type sourceType)
        {
            return new ContainerBuilder(this, sourceType);
        }

        //returns the instance which is mapped to the source provided.
        public TSource Resolve<TSource>()
        {
            return (TSource)Resolve(typeof(TSource));
        }
        public object Resolve(Type sourceType)
        {
            if (_map.ContainsKey(sourceType))
            {
                var destination = _map[sourceType];
                return CreateInstance(destination);
            }
            else if(sourceType.IsGenericType &&
                _map.ContainsKey(sourceType.GetGenericTypeDefinition()))
            {
                var destination = _map[sourceType.GetGenericTypeDefinition()];
                var closedDestination = destination.MakeGenericType(sourceType.GenericTypeArguments);
                return CreateInstance(closedDestination);
            }
            else if (!sourceType.IsAbstract)
            {
                return CreateInstance(sourceType);
            }
            else
            {
                throw new InvalidOperationException("Could not resovle");
            }
        }

        private object CreateInstance(Type destination)
        {
            var parameters = destination.GetConstructors()
                .OrderByDescending(c => c.GetParameters().Count())
                .First() // may return an exception if no pulic constructor.
                .GetParameters()
                //here the ILogger need to be created by the container
                //as long as the mappings have been set before hand.
                .Select(p => Resolve(p.ParameterType))
                .ToArray();
            return Activator.CreateInstance(destination, parameters);
        }

        public class ContainerBuilder
        {
            public ContainerBuilder(Container container, Type sourceType)
            {
                _container = container;
                _type = sourceType;
            }
            Container _container;
            Type _type;
            
            public ContainerBuilder Use<TDestination>()
            {
                return Use(typeof(TDestination));
            }
            public ContainerBuilder Use(Type destinationType)
            {
                _container._map.Add(_type, destinationType);
                //this is in case we need to chain in the future.
                return this;
            }
        }
    }
}
