using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodleEnroller.Common.Entities
{
    public class MoodleEnrollment
    {
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
    }
}
