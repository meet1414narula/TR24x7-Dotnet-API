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
  
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        #region Private variable.

        private readonly ITokenService _tokenServices;
        private readonly IUserService _userService;
       // private ResponseEntity responseEntity = null;
        #endregion

        #region Public Constructor

        /// <summary>
        /// Public constructor to initialize product service instance
        /// </summary>
        public AccountController(ITokenService tokenServices, IUserService userService)
        {
            _tokenServices = tokenServices;
            _userService = userService;
        }

        #endregion

        // POST api/Account/Register
        [HttpPost]
        [HttpOptions]
        [ActionName("Register")]
        public HttpResponseMessage Register([FromBody] RegisterEntity registerEntity)
        {
            if (registerEntity == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            var userId = _userService.Create(registerEntity);
            var userEntity = new UserEntity {UserId=userId,MobileNumber=registerEntity.MobileNumber,Password=registerEntity.Password };
            return GetAuthToken(userEntity);
        }


        // POST api/Account/Login
        [HttpPost]
        [HttpOptions]
        [ActionName("Login")]
        public HttpResponseMessage Login([FromBody] UserEntity userEntity)
        {
            if (userEntity == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            userEntity.UserId = _userService.Authenticate(Convert.ToInt64(userEntity.MobileNumber), userEntity.Password);
            return GetAuthToken(userEntity); //Request.CreateResponse(HttpStatusCode.OK, userEntity);
        }

        [HttpPost]
        [HttpOptions]
        [ActionName("ForgotPassword")]
        public HttpResponseMessage ForgotPassword([FromBody] UserEntity userEntity)
        {
            if (userEntity == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            userEntity.UserId = _userService.GetUserId(Convert.ToInt64(userEntity.MobileNumber));
            return GetAuthToken(userEntity);
        }

        [HttpPost]
        [HttpOptions]
        [ActionName("ChangePassword")]
        public HttpResponseMessage ChangePassword([FromBody] UserEntity userEntity)
        {
            if (userEntity == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            userEntity.UserId = _userService.GetUserId(Convert.ToInt64(userEntity.MobileNumber));
            return GetAuthToken(userEntity);
        }


        /// <summary>
        /// Returns auth token for the validated user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private HttpResponseMessage GetAuthToken(UserEntity userEntity)
        {
            var token = _tokenServices.GenerateToken(userEntity.UserId);
            userEntity.Token = token.AuthToken;
            var response = Request.CreateResponse(HttpStatusCode.OK, userEntity);
            response.Headers.Add("UserId", Convert.ToString(userEntity.UserId));
            response.Headers.Add("Token", token.AuthToken);
            response.Headers.Add("TokenExpiry",Convert.ToString(token.ExpiresOn));
           //response.Headers.Add("Access-Control-Expose-Headers", "Token,TokenExpiry" );
            return response;
        }



        /// <summary>
        /// Authenticates user and returns token with expiry.
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        //[HttpOptions]
        //[ActionName("Login")]
        ////   [ApiAuthenticationFilter(true)]
        //////[POST("login")]
        //////[POST("authenticate")]
        //////[POST("get/token")]
        //public HttpResponseMessage Login()
        //{
        //    if (System.Threading.Thread.CurrentPrincipal != null && System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated)
        //    {
        //        var basicAuthenticationIdentity = System.Threading.Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
        //        if (basicAuthenticationIdentity != null)
        //        {
        //            var userId = basicAuthenticationIdentity.UserId;

        //            return GetAuthToken(userId);
        //        }
        //    }
        //    return null;
        //}
    }
}
