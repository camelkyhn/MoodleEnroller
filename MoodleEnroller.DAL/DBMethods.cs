using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodleEnroller.DAL
{
    public class DBMethods
    {
        public static MySqlConnection GetConnection()
        {
            //Database connection
            return new MySqlConnection("Server=localhost;Port=3306;Database=moodle570;Uid=root;Pwd=camel1913;");
        }

        public static MySqlCommand GetCommand(MySqlConnection connection, string tableName)
        {
            //Database command
            return new MySqlCommand("SELECT * From " + tableName, connection);
        }
    }
}
