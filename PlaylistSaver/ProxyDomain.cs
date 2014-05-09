using System;
using System.Diagnostics;
using System.Reflection;

namespace PlaylistSaver
{
    public class ProxyDomain
    {
        public Assembly GetAssembly(string assemblyPath)
        {
            try
            {
                return Assembly.LoadFrom(assemblyPath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}
