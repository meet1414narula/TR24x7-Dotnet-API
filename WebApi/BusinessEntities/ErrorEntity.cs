using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessEntities
{
    public class ErrorEntity
    {
        public string Message { get; set; }
        public DateTime CreationDate { get; set; }
        public string TargetSite { get; set; }
        public string StackTrace { get; set; }
        public long UserId { get; set; }
    }
}
