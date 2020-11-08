using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessServices;
using WebApi.Filters;
using System;
using System.Threading;
using BusinessEntities;

namespace WebApi.Controllers
{
    public class HelperController : ApiController
    {
        #region Private variable.

        private readonly IHelperService _helperServices;
       
        #endregion

        #region Public Constructor

        /// <summary>
        /// Public constructor to initialize product service instance
        /// </summary>
        public HelperController(IHelperService helperService)
        {
            _helperServices = helperService;
        }

        #endregion

        // POST api/Helper/Register
       
        [HttpGet]
        [HttpOptions]
       // [ActionName("GetStaticData")]
        public HttpResponseMessage GetStaticData()
        {
            var response= _helperServices.GetStaticData();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
