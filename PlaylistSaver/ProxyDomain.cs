using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
