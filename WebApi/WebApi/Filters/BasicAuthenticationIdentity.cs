using System.Security.Principal;

namespace WebApi.Filters
{
    /// <summary>
    /// Basic Authentication identity
    /// </summary>
    public class BasicAuthenticationIdentity : GenericIdentity
    {
        /// <summary>
        /// Get/Set for password
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Get/Set for UserName
        /// </summary>
        public string MobileNumber { get; set; }
        /// <summary>
        /// Get/Set for UserId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Basic Authentication Identity Constructor
        /// </summary>
        /// <param name="mobileNumber"></param>
        /// <param name="password"></param>
        public BasicAuthenticationIdentity(string mobileNumber, string password)
            : base("", "Basic")
        {
            Password = password;
            MobileNumber = mobileNumber;
        }
    }
}