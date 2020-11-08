using System.Threading;
using System.Web.Http.Controllers;
using BusinessServices;
using System;

namespace WebApi.Filters
{
    /// <summary>
    /// Custom Authentication Filter Extending basic Authentication
    /// </summary>
    public class ApiAuthenticationFilter : GenericAuthenticationFilter
    {
        /// <summary>
        /// Default Authentication Constructor
        /// </summary>
        public ApiAuthenticationFilter()
        {
        }

        /// <summary>
        /// AuthenticationFilter constructor with isActive parameter
        /// </summary>
        /// <param name="isActive"></param>
        public ApiAuthenticationFilter(bool isActive)
            : base(isActive)
        {
        }

        /// <summary>
        /// Protected overriden method for authorizing user
        /// </summary>
        /// <param name="mobileNumber"></param>
        /// <param name="password"></param>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected override bool OnAuthorizeUser(string mobileNumber, string password, HttpActionContext actionContext)
        {
            var userService = actionContext.ControllerContext.Configuration
                               .DependencyResolver.GetService(typeof(IUserService)) as IUserService;
            if (userService != null)
            {
                var userId = userService.Authenticate(Convert.ToInt64(mobileNumber), password);
                if (userId>0)
                {
                    var basicAuthenticationIdentity = Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
                    if (basicAuthenticationIdentity != null)
                        basicAuthenticationIdentity.UserId = userId;
                    return true;
                }
            }
            return false;
        }
    }
}