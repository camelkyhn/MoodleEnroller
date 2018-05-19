using MoodleEnroller.BLL.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodleEnroller.Main
{
    class Program
    {
        static void Main(string[] args)
        {
            string token = ConfigurationManager.AppSettings["Token"].ToString(); //Check App.config file for Moodle's Admin token

            MoodleUserRepository repository = new MoodleUserRepository();

            repository.CoreUserCreateUsers(token);

            repository.EnrolManualEnrolUsers(token);
        }
    }
}
