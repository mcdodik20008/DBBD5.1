using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore.
namespace BD_ITOG
{
    public static class SQL
    {
        public static List<List<string>> ReadSql(string sqlExpression)
        {
            string connectionString = @"Data Source=KOMPYTER-ALEKSE\SQLEXPRESS;Initial Catalog=InSy;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            var command = new SqlCommand(sqlExpression, connection);
            var reader = command.ExecuteReader();

            var table = new List<List<string>>();
            var row = new List<string>();
            int nRows = reader.FieldCount;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    for (int i = 0; i < nRows; i++)
                    {
                        row.Add(reader.GetValue(i).ToString());
                    }
                    table.Add(row);
                    row = new List<string>();
                }
            }
            reader.Close();
            connection.Close();
            return table;
        }
        public static int maxIndex(string command)
        {
            return int.Parse(ReadSql(command)[0][0]);
        }
        // раньше нужен был, сейчас нет. Пускай пока полежит
        public static string searchIn2xTable(List<List<string>> table, string inTable)
        {
            if (inTable == "") return "";
            int fk_book = 0;
            foreach (var x in table)
            {
                if (x.IndexOf(inTable) != -1) return table[fk_book][0];
                fk_book += 1;
            }
            return fk_book + "";
        }

        //действия с таблицой - сохранение итд
        public static void InteractingSql(string sqlExpression)
        {
            string connectionString = @"Data Source=KOMPYTER-ALEKSE\SQLEXPRESS;Initial Catalog=InSy;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            int number = command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
