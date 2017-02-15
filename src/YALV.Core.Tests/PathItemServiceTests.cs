using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using YALV.Core.Domain;

namespace YALV.Core
{
    [TestFixture]
    public class PathItemServiceTests
    {
        private PathItemService _pathItemService;
        private string _testPath;

        [OneTimeSetUp]
        protected void Setup()
        {
            _pathItemService = new PathItemService();
            _testPath = Path.Combine(TestContext.CurrentContext.TestDirectory, Path.GetRandomFileName());
        }

        [OneTimeTearDown]
        protected void Teardown()
        {
            File.Delete(_testPath);
        }

        [Test]
        public void CanSaveAndLoadPathItems()
        {
            var items = new List<PathItem>
            {
                new PathItem("test1", @"c:\test\Path1"),
                new PathItem("test2", @"c:\program\logs")
            };

            _pathItemService.SaveFolderFile(items, _testPath);
            var actualItems = _pathItemService.ParseFolderFile(_testPath);

            Assert.That(actualItems, Is.EquivalentTo(items).Using(new Func<PathItem, PathItem, bool>(ComparePathItems)));
        }

        [Test]
        public void UpdatesPathItems()
        {
            var items = new List<PathItem>
            {
                new PathItem("test1", @"c:\test\Path1"),
                new PathItem("test2", @"c:\program\logs")
            };
            _pathItemService.SaveFolderFile(items, _testPath);

            _pathItemService.SaveFolderFile(new List<PathItem>(), _testPath);
            var actualItems = _pathItemService.ParseFolderFile(_testPath);

            Assert.That(actualItems, Is.Empty);
        }

        [Test]
        public void CanUpdateFromOldFormat()
        {
            var expected = new List<PathItem>()
            {
                new PathItem("SaveEvents", @"\\network\C$\SaveEvents\Logs"),
                new PathItem("Test", @"c:\temp"),
            };
            File.WriteAllLines(_testPath, new[]
            {
                 @"<folder name=""SaveEvents"" path=""\\network\C$\SaveEvents\Logs"" />",
                 @"<folder name=""Test"" path=""c:\temp"" />"
             });
            var actualItems = _pathItemService.ParseFolderFile(_testPath);

            Assert.That(actualItems, Is.EquivalentTo(expected).Using(new Func<PathItem, PathItem, bool>(ComparePathItems)));
        }

        private static bool ComparePathItems(PathItem x, PathItem y)
        {
            var allNotEqual = typeof(PathItem).GetProperties()
                .Select(propertyInfo => new { PropertyName = propertyInfo.Name, xValue = propertyInfo.GetValue(x), yValue = propertyInfo.GetValue(y) })
                .Where(values => !values.xValue.Equals(values.yValue))
                .ToList();

            if (!allNotEqual.Any())
                return true;

            foreach (var values in allNotEqual)
            {
                var xText = values.xValue ?? "<null>";
                var yText = values.yValue ?? "<null>";
                TestContext.WriteLine($"For property '{values.PropertyName}' Expected <{xText}> but found <{yText}>");
            }
            return false;
        }
    }
}
