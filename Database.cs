using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ConsoleApp1
{
    class Database
    {
        public MySqlConnection Connection { get; set; }

        public Database(string connectionString)
        {
            this.Connection = new MySqlConnection(connectionString);
        }
      
        public  Database(MySqlConnection sql)
        {
           
            try
            {
                this.Connection = sql;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }

        public void Open()
        {
            Connection.Open();
        }
        public void Close()
        {
            Connection.Close();
        }

        public void Execute(string query)
        {

            MySqlCommand command = new MySqlCommand(query, this.Connection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                // элементы массива [] - это значения столбцов из запроса SELECT
                Console.WriteLine(reader[0].ToString() + " " + reader[1].ToString());
            }
            reader.Close(); 


        }

        public Query GetQuery(string table)
        {
            Query query = new Query(table);

            return query;
        }

    }
}
