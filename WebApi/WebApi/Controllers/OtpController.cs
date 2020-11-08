using BusinessEntities;
using BusinessServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api/Otp")]
    public class OtpController : ApiController
    {
        #region Private variable.

        private readonly IOTPService _otpService;

        #endregion

        #region Public Constructor

        /// <summary>
        /// Public constructor to initialize otp service instance
        /// </summary>
        public OtpController(IOTPService otpService)
        {
            _otpService = otpService;
        }

        #endregion

        #region Public methods
        // URL api/otp/generate
        [HttpGet]
        [HttpOptions]
        [ActionName("Generate")]
        public HttpResponseMessage Generate(string mobileNumber)
        {
            _otpService.Generate(Convert.ToInt64(mobileNumber)) ;
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // URL api/otp/generate
        //[HttpPost]
        //[HttpOptions]
        //[ActionName("Verify")]
        //public HttpResponseMessage Verify(OTPDetails otpDetails)
        //{
        //    _otpService.Verify(Convert.ToInt32(otpDetails.otpNumber));
        //    return Request.CreateResponse(HttpStatusCode.OK);
        //}

        [HttpPost]
        [HttpOptions]
        [ActionName("Verify")]
        public HttpResponseMessage Verify([FromBody]string otpNumber)
        {
            if (otpNumber == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            _otpService.Verify(Convert.ToInt32(otpNumber));
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        public class OTPDetails
        {
            public string otpNumber { get; set; }
        }

        #endregion
    }
}
