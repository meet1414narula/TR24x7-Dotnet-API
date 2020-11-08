using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessServices
{
    public  interface IOTPService
    {
        void Generate(long mobileNumber);
        void Verify(int otpNumber);
    }
}
