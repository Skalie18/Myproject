using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System;
namespace PushCBCData.BO
{
    public class DefaultFileParser : IFileParser
    {

        private readonly int _chunkRowLimit;
        private readonly string _connectionString;

        public DefaultFileParser()
        {
            _chunkRowLimit = 3;//TODO:configurable
            //TODO:read from config file
            _connectionString = AppConfigs.ConnextionString;
        }

        IEnumerable<DataTable> IFileParser.GetFileData(string sourceFileFullName)
        {
            bool firstLineOfChunk = true;
            int chunkRowCount = 0;
            DataTable chunkDataTable = null;
            string columnData = null;
            bool firstLineOfFile = true;

            using (var sr = new StreamReader(sourceFileFullName))
            {
                string line = null;
                //Read and display lines from the file until the end of the file is reached.                
                while ((line = sr.ReadLine()) != null)
                {
                    //when reach first line it is column list need to create datatable based on that.
                    if (firstLineOfFile)
                    {
                        columnData = line;
                        firstLineOfFile = false;
                        continue;
                    }
                    if (firstLineOfChunk)
                    {
                        firstLineOfChunk = false;
                        chunkDataTable = CreateEmptyDataTable(columnData);
                    }
                    AddRow(chunkDataTable, line);
                    chunkRowCount++;

                    if (chunkRowCount == _chunkRowLimit)
                    {
                        firstLineOfChunk = true;
                        chunkRowCount = 0;
                        yield return chunkDataTable;
                        chunkDataTable = null;
                    }
                }
            }
            //return last set of data which less then chunk size
            if (null != chunkDataTable)
                yield return chunkDataTable;

        }

        private DataTable CreateEmptyDataTable(string firstLine)
        {
            try
            {
                IList<string> columnList = Split(firstLine);
                var dataTable = new DataTable("MultiNationalEntityListTemp");
                dataTable.Columns.AddRange(columnList.Select(v => new DataColumn(v)).ToArray());
                return dataTable;
            }
            catch (Exception x)
            {
                EventLogging.LogError("Error Msg : " + x.Message + "\n" +
                               "Source : " + x.Source + "\n" +
                               "Event : CreateEmptyDataTable");
            }

            return null;
        }

        private void AddRow(DataTable dataTable, string line)
        {
            try
            {
                DataRow newRow = dataTable.NewRow();
                IList<string> fieldData = Split(line);
                for (int columnIndex = 0; columnIndex < dataTable.Columns.Count; columnIndex++)
                {
                    newRow[columnIndex] = fieldData[columnIndex];
                }
                dataTable.Rows.Add(newRow);
            }
            catch (Exception x)
            {
                EventLogging.LogError("Error Msg : " + x.Message + "\n" +
                               "Source : " + x.Source + "\n" +
                               "Event : AddRow");
            }
        }

        private IList<string> Split(string input)
        {
            try
            {
                //our csv file will be tab delimited
                var dataList = new List<string>();
                //foreach (string column in input.Split('\t'))
                foreach (string column in input.Split('|'))
                {
                    dataList.Add(column);
                }
                return dataList;
            }
            catch (Exception x)
            {
                EventLogging.LogError("Error Msg : " + x.Message + "\n" +
                               "Source : " + x.Source + "\n" +
                               "Event : Split");
            }

            return null;
        }

        void IFileParser.WriteChunkData(DataTable table, string distinationTable, IList<KeyValuePair<string, string>> mapList)
        {
            try
            {
                using (var bulkCopy = new SqlBulkCopy(_connectionString, SqlBulkCopyOptions.Default))
                {
                    bulkCopy.BulkCopyTimeout = 0;//unlimited
                    bulkCopy.DestinationTableName = distinationTable;
                    foreach (KeyValuePair<string, string> map in mapList)
                    {
                        bulkCopy.ColumnMappings.Add(map.Key, map.Value);
                    }
                    bulkCopy.WriteToServer(table, DataRowState.Added);
                }
            }
            catch (Exception x)
            {
                EventLogging.LogError("Error Msg : " + x.Message + "\n" +
                               "Source : " + x.Source + "\n" +
                               "Event : WriteChunkData");
            }
        }

    }
}
