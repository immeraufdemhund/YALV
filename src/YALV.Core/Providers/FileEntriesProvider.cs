using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using YALV.Core.Domain;
using YALV.Core.Exceptions;

namespace YALV.Core.Providers
{
    public class FileEntriesProvider : AbstractEntriesProvider
    {
        private readonly IReadFileLineByLine _fileReader;
        private const string SEPARATOR = "[---]";
        private const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss,fff";

        public FileEntriesProvider()
            : this(new ReadFileLineByLine())
        {
        }
        public FileEntriesProvider(IReadFileLineByLine fileReader)
        {
            _fileReader = fileReader;
        }

        public override IEnumerable<LogItem> GetEntries(string dataSource, FilterParams filter)
        {
            ValidateFilterParameters(filter);

            var pattern = filter.Pattern;
            var file = new FileInfo(dataSource);
            var regex = new Regex(@"%\b(date|message|level)\b");
            var matches = regex.Matches(pattern);

            foreach (var s in _fileReader.Read(file))
            {
                var items = s.Split(new[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
                var entry = CreateEntry(items, matches);
                entry.Logger = filter.Logger;
                yield return entry;
            }
        }

        private static void ValidateFilterParameters(FilterParams filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            if (string.IsNullOrEmpty(filter.Pattern))
                throw new NotValidValueException("filter pattern null");
        }

        private static LogItem CreateEntry(string[] items, MatchCollection matches)
        {
            if (items.Length != matches.Count)
                throw new NotValidValueException("different length of items/matches values");

            var entry = new LogItem();
            for (var i = 0; i < matches.Count; i++)
            {
                var value = items[i];
                var match = matches[i];
                var name = match.Value;
                switch (name)
                {
                    case "%date":
                        entry.TimeStamp = DateTime.ParseExact(
                            value, DATE_TIME_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                        break;

                    case "%message":
                        entry.Message = value;
                        break;

                    case "%level":
                        entry.Level = value;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(name, "unmanaged value");
                }
            }
            return entry;
        }
    }
}