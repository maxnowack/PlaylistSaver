using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistSaver
{
    /// <summary>
    /// Method cache for Factory
    /// </summary>
    public static class FactoryCache
    {
        private static List<Type> collectedTypes;
        public static Dictionary<Type, List<MethodInfo>> MethodCache = new Dictionary<Type, List<MethodInfo>>();

        public static List<Type> GetTypes()
        {
            if (collectedTypes == null)
                UpdateTypes();
            if (collectedTypes != null)
                collectedTypes.ForEach(t => Debug.WriteLine(t.ToString() + ", "));
            return collectedTypes;
        }

        public static void AddTypes(IEnumerable<Type> types)
        {
            if (types == null)
                return;
            if (collectedTypes == null)
                collectedTypes = new List<Type>();
            foreach (var t in types)
            {
                if (!collectedTypes.Contains(t))
                    collectedTypes.Add(t);
            }
        }

        public static void AddTypes(IEnumerable<Assembly> assemblies)
        {
            foreach (var a in assemblies)
            {
                var typen = a.GetTypes();
                AddTypes(typen);
            }
        }

        public static void UpdateTypes()
        {
            collectedTypes = new List<Type>();
            try
            {
                var pd = new ProxyDomain();
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (string.IsNullOrEmpty(path))
                    return;
                Debug.WriteLine("Factory path: " + path);
                foreach (var file in Directory.GetFiles(path, "*.dll"))
                {
                    if (pd.GetAssembly(file) != null)
                        collectedTypes.AddRange(pd.GetAssembly(file).GetTypes());
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
