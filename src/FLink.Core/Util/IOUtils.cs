using System;
using System.Collections.Generic;
using FLink.Core.IO;

namespace FLink.Core.Util
{
    /// <summary>
    /// An utility class for I/O related functionality.
    /// </summary>
    public sealed class IOUtils
    {
        public static void CloseAllQuietly(IEnumerable<ICloseable> closeables)
        {
            foreach (var closeable in closeables)
                CloseQuietly(closeable);
        }

        public static void CloseQuietly(ICloseable closeable)
        {
            try
            {
                closeable?.Close();
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
