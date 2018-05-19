using MoodleEnroller.Common.Entities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MoodleEnroller.DAL.Database
{
    public class MoodleUsers
    {
        public static List<MoodleUser> GetUserList()
        {
            List<MoodleUser> userList = new List<MoodleUser>();

            var connection = DBMethods.GetConnection();
            var command = DBMethods.GetCommand(connection, "Users");
            int i = 0; // Row index for DataReader

            try
            {
                MySqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    MoodleUser user = new MoodleUser();

                    if (dataReader.IsDBNull(i))
                    {
                        Console.WriteLine("Null!");
                        break;
                    }
                    else
                    {
                        //necessary fields to create the user
                        user.Id = dataReader.GetInt32(0); //This will not going to be added in to post data.
                        user.Username = HttpUtility.UrlEncode(dataReader.GetString(1));
                        user.Password = HttpUtility.UrlEncode(dataReader.GetString(2)); //Let's assume we've got student's password in db for him/her moodle account
                        user.FirstName = HttpUtility.UrlEncode(dataReader.GetString(3));
                        user.LastName = HttpUtility.UrlEncode(dataReader.GetString(4));
                        user.Email = HttpUtility.UrlEncode(dataReader.GetString(5));

                        userList.Add(user);
                    }
                    i++;
                }

                //Close the reader.
                dataReader.Close();
            }
            finally
            {
                //Close the conection
                connection.Close();
            }

            return userList;
        }
    }
}
