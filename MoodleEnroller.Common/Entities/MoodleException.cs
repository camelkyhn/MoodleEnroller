using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodleEnroller.Common.Entities
{
    public class MoodleException
    {
        public string Exception { get; set; }
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string DebugInfo { get; set; }
    }
}
