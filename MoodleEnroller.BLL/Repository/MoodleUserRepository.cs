using MoodleEnroller.Common.Entities;
using System;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MoodleEnroller.DAL;

namespace MoodleEnroller.BLL.Repository
{
    public class MoodleUserRepository
    {
        List<MoodleUser> UserList;
        List<MoodleEnrollment> EnrollmentList;

        public MoodleUserRepository()
        {
            UserList = MoodleUsers.GetUserList();
            EnrollmentList = MoodleEnrollments.GetEnrollmentList();
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

        public void EnrolManualEnrolUsers(string token)
        {
            string enrolRequest = GetEnrolRequest(token);

            foreach (var enrollment in EnrollmentList)
            {
                string enrolData = GetEnrolData(enrollment);
                PostEnrolRequest(enrolRequest, enrolData);
            }
        }

        public string GetCreateData(MoodleUser user)
        {
            //Post data for creation
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
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(createRequest);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            // Encode the parameters as form data:
            byte[] formData = UTF8Encoding.UTF8.GetBytes(postData);
            request.ContentLength = formData.Length;

            // Write out the form Data to the request:
            using (Stream post = request.GetRequestStream())
            {
                post.Write(formData, 0, formData.Length);
            }

            // Get the Response
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
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

        public string GetEnrolData(MoodleEnrollment enrollment)
        {
            //Post data for enrollment
            return string.Format("enrolments[0][roleid]={0}&enrolments[0][userid]={1}&enrolments[0][courseid]={2}", enrollment.RoleId, enrollment.UserId, enrollment.CourseId);
        }

        public string GetEnrolRequest(string token)
        {
            //Request to enrol the user with admin's token
            return string.Format("http://localhost:55166/webservice/rest/server.php?wstoken={0}&wsfunction={1}&moodlewsrestformat=json", token, "enrol_manual_enrol_users");
        }
        
        public void PostEnrolRequest(string enrolRequest, string enrolData)
        {
            // Call Moodle REST Service
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(enrolRequest);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            // Encode the parameters as form data:
            byte[] formData = UTF8Encoding.UTF8.GetBytes(enrolData);
            request.ContentLength = formData.Length;

            // Write out the form Data to the request:
            using (Stream post = request.GetRequestStream())
            {
                post.Write(formData, 0, (int)request.ContentLength);
            }

            // Get the Response
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(resStream);
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
                //List<MoodleCreateUserResponse> newUsers = serializer.Deserialize<List<MoodleCreateUserResponse>>(contents);

                //Check the result
                Console.WriteLine(contents);
            }
        }
    }
}
