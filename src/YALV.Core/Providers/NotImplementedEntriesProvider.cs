using System;
using System.Collections.Generic;
using YALV.Core.Domain;

namespace YALV.Core.Providers
{
    internal class NotImplementedEntriesProvider : AbstractEntriesProvider
    {
        private readonly EntriesProviderType _entriesProviderType;

        public NotImplementedEntriesProvider(EntriesProviderType entriesProviderType)
        {
            _entriesProviderType = entriesProviderType;
        }

        public override IEnumerable<LogItem> GetEntries(string dataSource, FilterParams filter)
        {
            yield return new LogItem
            {
                App = "YALV",
                Path = _entriesProviderType.ToString(),
                Level = "ERROR",
                TimeStamp = DateTime.Now,
                Message = "The format of this file has no matching provider implemented"
            };
        }
    }
}
