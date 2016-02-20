using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;

namespace Orzoo.Office
{
    /// <summary>
    /// Excel Reader（OleDb）废弃的
    /// </summary>
    public class ExcelOleDbReader
    {
        #region 通过OLEDB读取（已废弃）

        private static string GetConnectionString(string fileLocation)
        {
            var props = new Dictionary<string, string>();
            var fileExtension = Path.GetExtension(fileLocation);

            if (fileExtension == ".xls")
            {
                props["Provider"] = "Microsoft.Jet.OLEDB.4.0";
                props["Extended Properties"] = "Excel 8.0";
            }

            else if (fileExtension == ".xlsx")
            {
                props["Provider"] = "Microsoft.ACE.OLEDB.12.0;";
                props["Extended Properties"] = "Excel 12.0 XML";
            }
            props["Data Source"] = fileLocation;

            var sb = new StringBuilder();
            foreach (var prop in props)
            {
                sb.Append(prop.Key);
                sb.Append('=');
                sb.Append(prop.Value);
                sb.Append(';');
            }

            return sb.ToString();
        }


        // 废弃
        public static DataSet Read(string fileName)
        {
            DataSet ds = new DataSet();

            var connectionString = GetConnectionString(fileName);

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;

                // 获取所有的 sheet
                DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                // 循环所有的 sheet 读取数据
                foreach (DataRow dr in dtSheet.Rows)
                {
                    string sheetName = dr["TABLE_NAME"].ToString();

                    if (!sheetName.EndsWith("$"))
                        continue;

                    // 从 sheet 中获取所有的 row
                    cmd.CommandText = "SELECT * FROM [" + sheetName + "]";

                    DataTable dt = new DataTable();
                    dt.TableName = sheetName;

                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                    da.Fill(dt);

                    ds.Tables.Add(dt);
                }

                cmd = null;
                conn.Close();
            }

            return ds;
        }

        // 废弃
        //public static DataSet Read(HttpPostedFileBase file)
        //{
        //    var fileName = file.FileName;
        //    var fileLocation = GetTempPathName(fileName);

        //    if (File.Exists(fileLocation))
        //    {
        //        File.Delete(fileLocation);
        //    }

        //    return Read(fileLocation);
        //}

        #endregion

    }
}
