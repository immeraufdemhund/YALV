using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using YALV.Core.Domain;

namespace YALV.Core.Providers
{
    [TestFixture]
    public class FileEntriesProviderTests
    {
        private FileEntriesProvider _fileEntriesProvider;
        private FilterParams _filterParams;
        private MockFileReader _mockFile;

        [OneTimeSetUp]
        protected void Setup()
        {
            _mockFile = new MockFileReader();
            _fileEntriesProvider = new FileEntriesProvider(_mockFile);
            _filterParams = new FilterParams
            {
                Pattern = "%d{yyyyMMdd HH:mm:ss} :%-5p [%t] %c %m%n"
            };
        }

        [Test]
        public void WhenReadingEmptyFileEmptyLogItemsReturns()
        {
            _mockFile.Lines.Clear();

            var entries = _fileEntriesProvider.GetEntries("USES MOCK FILE", _filterParams).ToList();

            Assert.That(entries, Is.Empty);
        }

        [Test]
        public void WhenContainsFooterOnlyEmptyLogItemsReturns()
        {
            _mockFile.Lines.Add("[===============================================================================================]");
            _mockFile.Lines.Add("[===============================================================================================]");

            var entries = _fileEntriesProvider.GetEntries("USES MOCK FILE", _filterParams).ToList();

            Assert.That(entries, Is.Empty);
        }

        private class MockFileReader : IReadFileLineByLine
        {
            public IList<string> Lines { get; }

            public MockFileReader()
            {
                Lines = new List<string>();
            }
            public IEnumerable<string> Read(FileInfo file)
            {
                return Lines;
            }
        }
    }
}
