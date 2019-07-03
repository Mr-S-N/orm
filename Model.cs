using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Reflection;
namespace ConsoleApp1
{

    public class TypeAttribute : System.Attribute
    {
        public string Type { get; set; }
        public TypeAttribute(string name)
        {
            Type = name;
        }
    }
   abstract   class Model
    {
        public string DbName { get; set; }
        public string Table { get; set; }
        public string con { get; set; }
        private MySqlConnection connection;
        MySqlCommand cmd;
        public Model(string connStr)
        {
            this.con = connStr;
            this.connection = new MySqlConnection(connStr);
           
        }
        public void CreateDb(string name)
        {
            connection.Open();
            cmd = new MySqlCommand($"CREATE DATABASE IF NOT EXISTS `{name}`;", connection );
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        private string GetT<T>(string prop)
        {
            MemberInfo property = typeof(T).GetProperty(prop);
            var dd = property.GetCustomAttribute(typeof(TypeAttribute)) as TypeAttribute;
         
                if (dd!=null)
            {
                return dd.Type;
            }
            return null;
                


        }
        public void AddValues<T>(Model model)
        {
            string name = "";
            string value = "";
            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                string t = GetT<T>(prop.Name);
                if (t != null)
                {
                    name += $"{prop.Name},";
                    value += $"'{prop.GetValue(model, null)}',";
                }
               


            }
            name = name.Remove(name.Length - 1);
            value = value.Remove(value.Length - 1);
            //   Console.WriteLine($"INSERT INTO {DbName}.{Table} ({name}) values({value});");
            connection = new MySqlConnection(this.con += $"Database={this.DbName};");
            connection.Open();
            cmd = new MySqlCommand($"INSERT INTO {DbName}.{Table} ({name}) values({value});", connection);
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        public virtual void CreateTable<T> (string name,Model model)
        {
            string query = "";
          

            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                string t = GetT<T>(prop.Name);
                if (t!=null)
                {
                    query += $"\n{prop.Name} {t},";
                }
             //   Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(model, null));


            }
            query = query.Remove(query.Length - 1);

           
            connection = new MySqlConnection(this.con += $"Database={this.DbName};");
            connection.Open();
            cmd = new MySqlCommand($"CREATE TABLE {name}({query});", connection);
            cmd.ExecuteNonQuery();
            connection.Close();

        }

    }
}
