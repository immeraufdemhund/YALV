using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using YALV.Core.Domain;

namespace YALV.Core
{
    public interface IPathItemService
    {
        void SaveFolderFile(IList<PathItem> folders, string path);
        IList<PathItem> ParseFolderFile(string path);
    }

    public class PathItemService : IPathItemService
    {
        private readonly XmlSerializer _serializer;
        private readonly XmlWriterSettings _xmlWriterSettings;
        private readonly PathItemService_1_4 _oldPathItemService;

        public PathItemService()
        {
            _xmlWriterSettings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true,
            };
            _serializer = new XmlSerializer(typeof(List<PathItem>));
            _oldPathItemService = new PathItemService_1_4();
        }
        public void SaveFolderFile(IList<PathItem> folders, string path)
        {
            try
            {
                if (folders == null)
                    return;

                using (var xmlWriter = XmlWriter.Create(path, _xmlWriterSettings))
                {
                    _serializer.Serialize(xmlWriter, folders);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error saving Favorites list [{0}]:\r\n{1}\r\n{2}", path, ex.Message, ex.StackTrace);
                throw;
            }
        }

        public IList<PathItem> ParseFolderFile(string path)
        {
            var result = new List<PathItem>();
            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
                return null;

            try
            {
                using (var streamReader = fileInfo.OpenRead())
                {
                    result.AddRange((List<PathItem>) _serializer.Deserialize(streamReader));
                }
            }
            catch (InvalidOperationException)
            {
                result.AddRange(_oldPathItemService.ParseFolderFile(fileInfo));
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error parsing Favorites list [{0}]:\r\n{1}\r\n{2}", path, ex.Message, ex.StackTrace);
                throw;
            }

            return result;
        }
    }

    public class PathItemService_1_4
    {
        public IEnumerable<PathItem> ParseFolderFile(FileInfo fileInfo)
        {
            var settings = new XmlReaderSettings { ConformanceLevel = ConformanceLevel.Fragment };

            using (var fileStream = fileInfo.OpenRead())
            using (var xmlTextReader = XmlReader.Create(fileStream, settings))
            {
                while (xmlTextReader.Read())
                {
                    if ((xmlTextReader.NodeType != XmlNodeType.Element) || (xmlTextReader.Name != "folder"))
                        continue;

                    yield return new PathItem(xmlTextReader.GetAttribute("name"), xmlTextReader.GetAttribute("path"));
                }
            }
        }
    }
}
