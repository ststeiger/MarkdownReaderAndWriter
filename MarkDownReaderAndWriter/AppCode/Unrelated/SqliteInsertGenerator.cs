
namespace MarkDownReaderAndWriter
{


    public class SqliteInsertGenerator
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


    }


}
