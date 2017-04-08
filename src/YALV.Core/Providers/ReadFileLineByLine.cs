using System.Collections.Generic;
using System.IO;

namespace YALV.Core.Providers
{
    internal class ReadFileLineByLine : IReadFileLineByLine
    {
        public IEnumerable<string> Read(FileInfo file)
        {
            using (var reader = file.OpenText())
            {
                string s;
                while ((s = reader.ReadLine()) != null)
                {
                    yield return s;
                }
            }
        }
    }
}