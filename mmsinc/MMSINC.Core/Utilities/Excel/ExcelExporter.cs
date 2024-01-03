using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;

namespace MMSINC.Utilities.Excel
{
    [Obsolete("The other Excel Exporter is probably better.")]
    public class ExcelExporter
    {
        private readonly Dictionary<string, Func<object, object>> _valueParsers =
            new Dictionary<string, Func<object, object>>();

        public Dictionary<string, Func<object, object>> ValueParsers
        {
            get { return _valueParsers; }
        }

        public void ConvertDataReaderToWorksheet2(IDbCommand importCommand, string savePath, string sheetName)
        {
            ConsoleWriter.StartTimer();
            ConsoleWriter.Write("ConvertDataReaderToWorksheet2 start " + sheetName);

            var excelConnString = new OleDbConnectionStringBuilder();
            excelConnString.Provider = "Microsoft.ACE.OLEDB.12.0";
            excelConnString.DataSource = savePath;
            excelConnString["Extended Properties"] = "Excel 8.0;HDR=Yes";

            using (var dr = importCommand.ExecuteReader())
            {
                var columnCount = dr.FieldCount;

                // Delete existing file after we've been able to execute the command.
                // Need to delete before creating, otherwise an error's thrown.
                System.IO.File.Delete(savePath);

                using (var oleConn = new OleDbConnection(excelConnString.ToString()))
                using (var insertCmd = oleConn.CreateCommand())
                {
                    oleConn.Open();
                    var headers = new string[columnCount];
                    var formattedHeaders = new List<string>();
                    var qs = new List<string>();

                    for (var curCol = 0; curCol < dr.FieldCount; curCol++)
                    {
                        var name = dr.GetName(curCol);
                        headers[curCol] = name;
                        formattedHeaders.Add("[" + name + "]");
                        qs.Add("?");
                        insertCmd.Parameters.Add(name, OleDbType.LongVarChar, 20000);
                    }

                    using (var cmd = oleConn.CreateCommand())
                    {
                        cmd.CommandText = string.Format("create table {0}({1})", sheetName,
                            string.Join(",", formattedHeaders.Select(x => x + " longtext")));
                        cmd.ExecuteNonQuery();
                    }

                    insertCmd.CommandText = string.Format("insert into {0} ({1}) values ({2})", sheetName,
                        string.Join(",", formattedHeaders), string.Join(",", qs));
                    insertCmd.Prepare();

                    var rowIndex = 1; // starts at 1 since 0 is the header rows

                    var values = new object[columnCount];

                    while (dr.Read())
                    {
                        if (rowIndex % 100 == 0)
                        {
                            Console.Write("\rRows processed from db: " + rowIndex);
                        }

                        dr.GetValues(values);
                        for (var i = 0; i < columnCount; i++)
                        {
                            var name = headers[i];
                            var val = values[i];
                            Func<object, object> accessor;
                            if (ValueParsers.TryGetValue(name, out accessor))
                            {
                                val = accessor(val) ?? DBNull.Value;
                            }

                            insertCmd.Parameters[headers[i]].Value = val;
                        }

                        insertCmd.ExecuteNonQuery();
                        rowIndex++;
                    }

                    insertCmd.Dispose();
                }

                Console.WriteLine(""); // Ends the progress, need to move to the next line properly.
                ConsoleWriter.Write("ConvertDataReaderToWorksheet end " + sheetName);
            }

            ConsoleWriter.KillTimer();
        }

        public static class ConsoleWriter
        {
            private static System.Diagnostics.Stopwatch _stopWatch;

            public static void StartTimer()
            {
                _stopWatch = System.Diagnostics.Stopwatch.StartNew();
            }

            public static void KillTimer()
            {
                if (_stopWatch != null)
                {
                    _stopWatch.Stop();
                }
            }

            public static long Ms()
            {
                if (_stopWatch != null)
                {
                    return _stopWatch.ElapsedMilliseconds;
                }

                return 0;
            }

            public static void Write(object message)
            {
                if (_stopWatch != null)
                {
                    Console.WriteLine("{0} ms {1}", Ms(), message);
                }
            }
        }
    }
}
