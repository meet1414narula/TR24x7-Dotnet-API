using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Configuration;
using System.Text;
using DataModel.UnitOfWork;
using Common.Constants;
using Newtonsoft.Json.Linq;
using DataModel;
using System.Threading.Tasks;
using Common.Helpers;
using Commom.ErrorHelpers;

namespace BusinessServices
{
    public class OTPServices : IOTPService
    {
        #region Private member variables.
        private readonly IUnitOfWork _unitOfWork;
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById(GenericConstant.INDIAN_STANDARD_TIME);
        private string mockOTPService;
        private string otpKey;
        private string senderId;
        private long adminMobileNumber;
        #endregion

        #region Public constructor.

        /// <summary>
        /// Public constructor.
        /// </summary>
        public OTPServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            mockOTPService = ConfigurationManager.AppSettings[GenericConstant.MOCK_OTP_SERVICE];
            otpKey = ConfigurationManager.AppSettings[GenericConstant.OTP_KEY];
            senderId = ConfigurationManager.AppSettings[GenericConstant.MESSAGE_SERVICE_SENDER_ID];
            adminMobileNumber =Convert.ToInt64(ConfigurationManager.AppSettings[GenericConstant.ADMIN_MOBILE_NUMBER]);
        }

        #endregion

        #region Public member methods.

        public void Generate(long mobileNumber)
        {
            try
            {
                Random generator = new Random();
                int randomNumber = generator.Next(100000, 1000000);
                
                var otpResponse = MessageHelper.SendMessageToMobile(adminMobileNumber, GenericConstant.TEMPLATE_REGISTRATION, new List<string> { randomNumber.ToString() });

                // Add otp to database
                AddToLocalDB(otpResponse, mobileNumber, randomNumber.ToString());

                // For sending message to admin as well
                //Task task = new Task(() => SendMessageToMobile(8447034867, GenericConstant.TEMPLATE_REGISTRATION, new List<string> { randomNumber.ToString() }));
                //task.Start();

            }
            catch (Exception ex)
            {
                //Add Error to local DB
                var error = new Error
                {
                    CreationDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE),
                    Message = ex.Message,
                    StackTrace =ex.StackTrace,
                    TargetSite = ex.TargetSite.ToString()
                };
                _unitOfWork.ErrorRepository.Insert(error);


                throw new ApiBusinessException(ErrorConstant.ErrorCode.ERR101, ErrorConstant.ErrorMessage.OTP_GENERATE);
            }
        }

        public void Verify(int otpNumber)
        {
            var otp = _unitOfWork.OTPRepository.GetSingle(x=> x.OTPNumber== otpNumber);
            if(otp==null)
            {
                throw new ApiBusinessException(ErrorConstant.ErrorCode.ERR102, ErrorConstant.ErrorMessage.OTP_INVALID);
            }
        }

        //public void SendMessageToMobile(long mobilenumber, string templatename, List<string> parameters)
        //{
        //    using (WebClient client = new WebClient())
        //    {
        //        string responsebody = null;
        //        var reqparm = new System.Collections.Specialized.NameValueCollection();
        //        reqparm.Add(GenericConstant.FROM, senderId);
        //        reqparm.Add(GenericConstant.TO, Convert.ToString(mobilenumber));
        //        reqparm.Add(GenericConstant.TEMPLATE_NAME, templatename);
        //        int length = parameters.Count();
        //        for (int i = 0; i < length; i++)
        //        {
        //            reqparm.Add(GenericConstant.MESSAGE_VARIABLE + (i + 1).ToString(), parameters[i]);
        //        }

        //        if (!mockOTPService.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
        //        {
        //            byte[] responsebytes = client.UploadValues(string.Format(GenericConstant.MESSAGE_SERVICE_URL, otpKey), GenericConstant.METHOD.POST, reqparm);
        //            responsebody = Encoding.UTF8.GetString(responsebytes);
        //        }
        //        else
        //        {
        //            responsebody = @"{'Status':'Success','Details':'9bc2239f - 124f - 11e7 - 9462 - 00163ef91450'}";
        //        }
        //        JObject jObject = JObject.Parse(responsebody);
              

        //        if (templatename.Equals(GenericConstant.TEMPLATE_REGISTRATION, StringComparison.InvariantCultureIgnoreCase) && mobilenumber != 8447034867)
        //        {
        //            AddToLocalDB(jObject, mobilenumber, parameters[0]);
        //        }

        //    }
        //}

        #endregion

        #region Private Methods

        private void AddToLocalDB(string otpResponse, long? mobilenumber, string otpValue)
        {
            JObject jObject = JObject.Parse(otpResponse);
            OTP otp = new OTP
            {
                MobileNumber = mobilenumber,
                CreationDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE),
                ExpiryDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE).AddYears(1),
                CreationStatus = (string)jObject[GenericConstant.STATUS],
                Details = (string)jObject[GenericConstant.DETAILS],
                NoOfTry = 0,
                OTPNumber = Convert.ToInt32(otpValue)
            };
            _unitOfWork.OTPRepository.Insert(otp);
            _unitOfWork.Save();
        }


        #endregion
    }
}
