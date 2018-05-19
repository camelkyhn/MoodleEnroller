using MoodleEnroller.Common.Entities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodleEnroller.DAL
{
    public class MoodleEnrollments
    {
        public static List<MoodleEnrollment> GetEnrollmentList()
        {
            List<MoodleEnrollment> enrollmentList = new List<MoodleEnrollment>();

            var connection = DBMethods.GetConnection();
            var command = DBMethods.GetCommand(connection, "Enrollments");
            int i = 0; // Row index for DataReader

            connection.Open();

            try
            {
                MySqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    if (dataReader.IsDBNull(i))
                    {
                        Console.WriteLine("Null!");
                        break;
                    }
                    else
                    {
                        MoodleEnrollment enrollment = new MoodleEnrollment()
                        {
                            UserId = dataReader.GetInt32(0),
                            CourseId = dataReader.GetInt32(1),
                            RoleId = dataReader.GetInt32(2)
                        };

                        enrollmentList.Add(enrollment);
                    }
                    i++;
                }
                //Close the reader
                dataReader.Close();
            }
            finally
            {
                //Close the connection
                connection.Close();
            }
            return enrollmentList;
        }
    }
}
