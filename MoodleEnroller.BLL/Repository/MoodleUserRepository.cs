using MoodleEnroller.Common.Entities;
using MoodleEnroller.DAL.Database;
using System;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MoodleEnroller.BLL.Repository
{
    public class MoodleUserRepository
    {
        List<MoodleUser> UserList;

        public MoodleUserRepository()
        {
            UserList = MoodleUsers.GetUserList();
        }

        public void CoreUserCreateUsers(string token)
        {
            string createRequest = GetCreateRequest(token);
            foreach (var user in UserList)
            {
                string postData = GetCreateData(user);
                PostCreateRequest(createRequest, postData);
            }
        }

        public string GetCreateData(MoodleUser user)
        {
            //Post data
            return string.Format(
                "users[0][username]={0}&users[0][password]={1}&users[0][firstname]={2}&users[0][lastname]={3}&users[0][email]={4}", 
                user.Username, 
                user.Password, 
                user.FirstName, 
                user.LastName, 
                user.Email
                );
        }

        public string GetCreateRequest(string token)
        {
            //Request to create the user with admin's token
            return string.Format("http://localhost:55166/webservice/rest/server.php?wstoken={0}&wsfunction={1}&moodlewsrestformat=json", token, "core_user_create_users");
        }

        public void PostCreateRequest(string createRequest, string postData)
        {
            // Call Moodle REST Service
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(createRequest);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";

            // Encode the parameters as form data:
            byte[] formData = UTF8Encoding.UTF8.GetBytes(postData);
            req.ContentLength = formData.Length;

            // Write out the form Data to the request:
            using (Stream post = req.GetRequestStream())
            {
                post.Write(formData, 0, formData.Length);
            }

            // Get the Response
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string contents = reader.ReadToEnd();

            // Deserialize
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            if (contents.Contains("exception"))
            {
                // Error
                MoodleException moodleError = serializer.Deserialize<MoodleException>(contents);
            }
            else
            {
                // Good
                List<MoodleCreateUserResponse> newUsers = serializer.Deserialize<List<MoodleCreateUserResponse>>(contents);
                //Check the results
                foreach (var createdUser in newUsers)
                {
                    Console.WriteLine("Created User => Id: " + createdUser.Id + " Username: " + createdUser.Username);
                }
            }
        }
    }
}
