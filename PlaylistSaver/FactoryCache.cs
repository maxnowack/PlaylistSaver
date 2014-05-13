using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace PlaylistSaver
{
    using System.Linq;

    /// <summary>
    /// Method cache for Factory
    /// </summary>
    public static class FactoryCache
    {
        private static List<Type> _collectedTypes;
        public static Dictionary<Type, List<MethodInfo>> MethodCache = new Dictionary<Type, List<MethodInfo>>();

        public static List<Type> GetTypes()
        {
            if (_collectedTypes == null)
                UpdateTypes();
            if (_collectedTypes != null)
                _collectedTypes.ForEach(t => Debug.WriteLine(t.ToString() + ", "));
            return _collectedTypes;
        }

        public static void AddTypes(IEnumerable<Type> types)
        {
            if (types == null)
                return;
            if (_collectedTypes == null)
                _collectedTypes = new List<Type>();
            foreach (var t in types.Where(t => !_collectedTypes.Contains(t)))
            {
                _collectedTypes.Add(t);
            }
        }

        public static void AddTypes(IEnumerable<Assembly> assemblies)
        {
            foreach (var typen in assemblies.Select(a => a.GetTypes()))
            {
                AddTypes(typen);
            }
        }

        public static void UpdateTypes()
        {
            _collectedTypes = new List<Type>();
            try
            {
                var pd = new ProxyDomain();
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (string.IsNullOrEmpty(path))
                    return;
                Debug.WriteLine("Factory path: " + path);
                foreach (var file in Directory.GetFiles(path, "*.dll").Where(file => pd.GetAssembly(file) != null))
                {
                    _collectedTypes.AddRange(pd.GetAssembly(file).GetTypes());
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.GetInnermostException());
            }
            MethodCache.Clear();
        }
    }
}
