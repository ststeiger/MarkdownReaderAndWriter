using System;
using System.Collections.Generic;
using System.Web;



// using Community.CsharpSqlite;
// using Community.CsharpSqlite.SQLiteClient;


namespace MarkDownReaderAndWriter.ajax
{
    /// <summary>
    /// Summary description for TestData
    /// </summary>
    public class TestData : IHttpHandler
    {



        public static string GetInsertCommand(System.Data.DataTable dt)
        {
            string retVal = null; ;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("INSERT INTO ");
            sb.Append(dt.TableName);
            sb.Append(" \r\n( \r\n     ");

            foreach (System.Data.DataColumn dc in dt.Columns)
            {
                sb.Append(dc.ColumnName);
                sb.Append(" ");
                sb.Append(System.Environment.NewLine);
                sb.Append("    ,");
            }
            sb.Remove(sb.Length - 8, 8);
            sb.Append(" ");
            sb.Append(System.Environment.NewLine);
            sb.Append(") \r\n");


            sb.Append("VALUES \r\n( \r\n     ");
            foreach (System.Data.DataColumn dc in dt.Columns)
            {
                sb.Append("@");
                sb.Append(dc.ColumnName);
                sb.Append(" ");
                sb.Append(System.Environment.NewLine);
                sb.Append("    ,");
            }


            sb.Remove(sb.Length - 8, 8);
            sb.Append(" ");
            sb.Append(System.Environment.NewLine);
            sb.Append(");");

            retVal = sb.ToString();
            // System.Console.WriteLine(retVal);
            sb.Length = 0;
            sb = null;

            return retVal;
        }



        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");

            
            System.Data.SqlClient.SqlConnectionStringBuilder csb = new System.Data.SqlClient.SqlConnectionStringBuilder();
            csb.InitialCatalog = "SqLite";
            csb.DataSource = Environment.MachineName;
            csb.IntegratedSecurity = true;

            string cmd = "SELECT * FROM T_SYS_Ref_MimeTypes";
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd, csb.ConnectionString);
            System.Data.DataTable dt = new System.Data.DataTable();
            da.Fill(dt);
            dt.TableName = "T_SYS_Ref_MimeTypes";
            System.Console.WriteLine(dt.Rows.Count);

            string JSON = Tools.Serialization.JSON.SerializePretty(dt);
            System.IO.File.WriteAllText(@"D:\MimeTypes.json", JSON, System.Text.Encoding.UTF8);

            return;

            /*
            SqliteConnection con = new SqliteConnection();

            string dbFilename = @"SqliteTest3.db";
            dbFilename = @"C:\Users\Administrator\Documents\Visual Studio 2015\Projects\csharp-sqlite\Community.CsharpSqlite.SQLiteClient\bin\Debug\SqliteTest3.db";
            string cs = string.Format("Version=3,uri=file:{0}", dbFilename);

            Console.WriteLine("Set connection String: {0}", cs);

            // if (System.IO.File.Exists(dbFilename)) System.IO.File.Delete(dbFilename);

            con.ConnectionString = cs;
            con.Open();
            
            //System.Data.IDbCommand cmd = con.CreateCommand();
            //cmd.CommandText = "CREATE TABLE TEST_TABLE ( COLA INTEGER, COLB TEXT, COLC DATETIME )";
            //cmd.ExecuteNonQuery();
            */

            /*
            System.Data.IDataReader reader = cmd.ExecuteReader();
            int r = 0;
            Console.WriteLine("Read the data...");
            while (reader.Read())
            {
                Console.WriteLine("  Row: {0}", r);
                int i = reader.GetInt32(reader.GetOrdinal("COLA"));
                Console.WriteLine("    COLA: {0}", i);

                string s = reader.GetString(reader.GetOrdinal("COLB"));
                Console.WriteLine("    COLB: {0}", s);

                DateTime dt = reader.GetDateTime(reader.GetOrdinal("COLC"));
                Console.WriteLine("    COLB: {0}", dt.ToString("MM/dd/yyyy HH:mm:ss"));

                r++;
            }
            */



            //SqliteCommand command = new SqliteCommand("PRAGMA table_info('TEST_TABLE')", con);
            /*
            string ins = GetInsertCommand(dt);


            SqliteCommand command = new SqliteCommand(ins, con);
            command.Prepare();
            con.Open();


            foreach (System.Data.DataRow dr in dt.Rows)
            {
                command.Parameters.Clear();

                foreach (System.Data.DataColumn dc in dt.Columns)
                {
                    command.Parameters.Add("@"+dc.ColumnName, dr[dc.ColumnName]);
                }

                
                command.ExecuteNonQuery();
            }
            // con.Close();
            
            Console.WriteLine("Close and cleanup...");
            con.Close();
            con = null;
            */
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


    }


}