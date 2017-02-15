using System;
using System.Collections.Generic;
using System.Linq;
using YALV.Core.Domain;
using YALV.Core.Providers;

namespace YALV.Core
{
    public static class DataService
    {
        public static IList<LogItem> ParseLogFile(string path)
        {
            IEnumerable<LogItem> result = null;
            try
            {
                AbstractEntriesProvider provider = EntriesProviderFactory.GetProvider();
                result = provider.GetEntries(path);
                return result.ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error parsing log file [{0}]:\r\n{1}\r\n{2}", path, ex.Message, ex.StackTrace);
                throw;
            }
        }
    }
}