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
        private string errorMessage = "";

        public DefaultFileParser()
        {
            _chunkRowLimit = Int32.Parse(ConfigurationManager.AppSettings["chunkLimit"].ToString());
            _connectionString = ConfigurationManager.ConnectionStrings["FDR"].ConnectionString;
        }

        IEnumerable<DataTable> IFileParser.GetFileData(string sourceFileFullName)
        {
            bool firstLineOfChunk = true;
            int chunkRowCount = 0;
            DataTable chunkDataTable = null;
            string columnData = null;
            bool firstLineOfFile = true;

            if (!File.Exists(sourceFileFullName))
            {
                errorMessage = "File does not exists";
                yield return null;
            }
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
            IList<string> columnList = Split(firstLine);
            var dataTable = new DataTable("MultiNationalEntityListTemp");
            dataTable.Columns.AddRange(columnList.Select(v => new DataColumn(v)).ToArray());
            //return dataTable;
            return SetDatatypes(dataTable);
        }

        private DataTable SetDatatypes(DataTable datatable)
        {
            foreach (DataColumn coloumn in datatable.Columns)
            {
                switch (coloumn.ColumnName)
                {
                    case "PartyID":
                    case "SalesAmt":
                        coloumn.DataType = typeof(decimal);
                        break;
                    case "TaxRefNo":
                    case "RegisteredName":
                    case "TradingName":
                    case "RegistrationNo":
                    case "HoldingCompanyName":
                    case "HoldingCompanyResidentOutsideSAInd":
                    case "HoldingCompanyResidencyCode":
                    case "HoldingCompanyTaxRefNo":
                    case "MasterLocalFileRequiredInd":
                    case "CBCReportRequired":
                        coloumn.DataType = typeof(string);
                        break;
                    case "FinancialYearEndDate":
                    case "Datestamp":
                        coloumn.DataType = typeof(DateTime);
                        break;
                    case "AssessmentYear":
                        coloumn.DataType = typeof(int);
                        break;
                }
            }
            return datatable;
        }
        private void AddRow(DataTable dataTable, string line)
        {
            DataRow newRow = dataTable.NewRow();
            try
            {
                IList<string> fieldData = Split(line);
                for (int columnIndex = 0; columnIndex < dataTable.Columns.Count; columnIndex++)
                {
                    int newIndex = columnIndex + 1;
                    var value = fieldData[columnIndex];
                    if (value == "")
                        newRow[columnIndex] = DBNull.Value;
                    else
                        newRow[columnIndex] = value;
                }
                dataTable.Rows.Add(newRow);
            }
            catch (Exception x)
            {
                errorMessage = x.Message;
            }
        }



        private IList<string> Split(string input)
        {
            //our csv file will be pipe delimited
            var dataList = new List<string>();
            try
            {
                foreach (string column in input.Split('|'))
                {
                    dataList.Add(column);
                }
                return dataList;
            }
            catch (Exception x)
            {
                errorMessage = x.Message;
            }
            return null;
        }

        void IFileParser.WriteChunkData(DataTable table, string distinationTable, IList<KeyValuePair<string, string>> mapList)
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



        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
            set
            {
                errorMessage = value;
            }
        }
    }
}
