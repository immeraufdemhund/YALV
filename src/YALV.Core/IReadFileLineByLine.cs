using System.Collections.Generic;
using System.IO;

namespace YALV.Core
{
    public interface IReadFileLineByLine
    {
        IEnumerable<string> Read(FileInfo file);
    }
}