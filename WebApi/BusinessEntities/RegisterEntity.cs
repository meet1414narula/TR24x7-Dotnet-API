using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessEntities
{
    public class RegisterEntity
    {
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string UserType { get; set; }
        public int OTP { get; set; }
    }
}
