using Common.Constants;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
  public class MessageHelper
    {
        private static string mockOTPService = ConfigurationManager.AppSettings["MockOTPService"];
        private static string otpKey = ConfigurationManager.AppSettings["OTPAPIKey"];
        private static string senderId = ConfigurationManager.AppSettings["MessageServiceSenderId"];
       
        public static string SendMessageToMobile(long mobilenumber, string templatename, List<string> parameters)
        {
            using (WebClient client = new WebClient())
            {
                string responsebody = null;
                var reqparm = new System.Collections.Specialized.NameValueCollection();
                reqparm.Add(GenericConstant.FROM, senderId);
                reqparm.Add(GenericConstant.TO, Convert.ToString(mobilenumber));
                reqparm.Add(GenericConstant.TEMPLATE_NAME, templatename);
                int length = parameters.Count();
                for (int i = 0; i < length; i++)
                {
                    reqparm.Add(GenericConstant.MESSAGE_VARIABLE + (i + 1).ToString(), parameters[i]);
                }

                if (!mockOTPService.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
                {
                    byte[] responsebytes = client.UploadValues(string.Format(GenericConstant.MESSAGE_SERVICE_URL, otpKey), GenericConstant.METHOD.POST, reqparm);
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                else
                {
                    responsebody = @"{'Status':'Success','Details':'9bc2239f - 124f - 11e7 - 9462 - 00163ef91450'}";
                }
                // JObject jObject = JObject.Parse(responsebody);
                return responsebody;
            }
        }
    }
}
