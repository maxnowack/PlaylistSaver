using System;

namespace PlaylistSaver
{
    public static class Extensions
    {
        public static Exception GetInnermostException(this Exception ex)
        {
            if (ex.InnerException == null)
                return ex;
            do
            {
                ex = ex.InnerException;
            } while (ex.InnerException != null);
            return ex;
        }
    }
}
