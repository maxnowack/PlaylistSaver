using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace PlaylistSaver
{
    /// <summary>
    /// Creates instances of classes, that implements given interface of T by using static 'Create' method
    /// </summary>
    /// <typeparam name="T">Interface type</typeparam>
    internal class Factory<T> where T : class
    {
        /// <summary>
        /// Returns a list of instances, that implement T
        /// </summary>
        /// <param name="args">Create-method parameters</param>
        /// <returns>List of instances of T</returns>
        public static IEnumerable<T> CreateInstanceList(object[] args = null)
        {
            var instanceList = new List<T>();
            foreach (var method in GetMethods(args))
            {
                var instance = method.Invoke(null, args) as T;
                if (instance != null)
                    instanceList.Add(instance);
            }
            return instanceList;
        }

        /// <summary>
        /// Returns an instance of a class, thats implements T
        /// </summary>
        /// <param name="args">Create-method parameters</param>
        /// <returns>Instance of T</returns>
        public static T CreateInstance(object[] args = null)
        {
            foreach (var method in GetMethods(args))
            {
                var instance = method.Invoke(null, args) as T;
                if (instance != null)
                    return instance;
            }
            return default(T);
        }

        /// <summary>
        /// Search for matching methods
        /// </summary>
        /// <param name="args">Create-method parameters</param>
        /// <returns>List of methods</returns>
        private static IEnumerable<MethodInfo> GetMethods(object[] args)
        {
            if (FactoryCache.MethodCache.ContainsKey(typeof(T)))
                return FactoryCache.MethodCache[typeof(T)];

            var methods = (from currentType in FactoryCache.GetTypes()
                           where currentType.GetInterfaces().Contains(typeof(T))
                           from method in currentType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                           where method.Name == "Create"
                           select method).ToList();

            if (args != null)
            {
                methods = (from method in methods
                           where ParameterMatch(method, args)
                           select method).ToList();
            }
            else
            {
                methods = (from method in methods
                           where !method.GetParameters().Any()
                           select method).ToList();
            }
            Debug.WriteLine("Factory found '{0}' methods to invoke", methods.Count);
            FactoryCache.MethodCache.Add(typeof(T), methods);
            return methods;
        }

        /// <summary>
        /// Checks if parameters of method match given arguments
        /// </summary>
        /// <param name="method">Current method</param>
        /// <param name="args">Given arguments</param>
        /// <returns>Matching result</returns>
        static bool ParameterMatch(MethodInfo method, object[] args)
        {
            if (method.GetParameters().Count() != args.Count())
                return false;

            var parameters = method.GetParameters();
            foreach (var param in parameters.OrderBy(p => p.Position))
            {
                if (!param.ParameterType.Equals(args[param.Position].GetType()))
                    return false;
            }
            return true;
        }
    }
}
