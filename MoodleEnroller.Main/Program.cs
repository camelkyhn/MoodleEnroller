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
            MoodleUserRepository repository = new MoodleUserRepository();

            repository.CoreUserCreateUsers(ConfigurationManager.AppSettings["Token"].ToString()); //Check App.config file for Moodle's Admin token
        }
    }
}
