using System.Collections.Generic;
using System.Data;
using System.Configuration;

namespace PushCBCData.BO
{
    public interface IFileParser
    {
        IEnumerable<DataTable> GetFileData(string sourceDirectory);
        void WriteChunkData(DataTable table, string distinationTable, IList<KeyValuePair<string, string>> mapList);
    }

}
