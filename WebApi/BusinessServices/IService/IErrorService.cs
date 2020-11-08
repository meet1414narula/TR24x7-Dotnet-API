using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessServices
{
    public  interface IErrorService
    {
        void AddError(ErrorEntity errorEntity);
    }
}
