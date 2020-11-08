using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessEntities
{
    public class UserEntity
    {
        public long UserId { get; set; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
    }
}
