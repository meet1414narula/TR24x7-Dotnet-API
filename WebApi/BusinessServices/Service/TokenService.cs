using System;
using System.Linq;
using BusinessEntities;
using DataModel;
using DataModel.UnitOfWork;
using Common.Constants;

namespace BusinessServices
{
    public class TokenService:ITokenService
    {
        #region Private member variables.
        private readonly IUnitOfWork _unitOfWork;
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById(GenericConstant.INDIAN_STANDARD_TIME);
        public object ConfigurationManager { get; private set; }
        #endregion

        #region Public constructor.
        /// <summary>
        /// Public constructor.
        /// </summary>
        public TokenService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Public member methods.

        /// <summary>
        ///  Function to generate unique token with expiry against the provided userId.
        ///  Also add a record in database for generated token.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public TokenEntity GenerateToken(long userId)
        {
            string token = Guid.NewGuid().ToString();
            DateTime issuedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            DateTime expiredOn = issuedOn.AddMinutes(
                                              Convert.ToDouble(20000));
            var tokendomain = new Token
                                  {
                                      UserFID = userId,
                                      Value = token,
                                      CreationDate = issuedOn,
                                      ExpiryDate = expiredOn
                                  };

            _unitOfWork.TokenRepository.Insert(tokendomain);
            _unitOfWork.Save();
            var tokenModel = new TokenEntity()
                                 {
                                     UserId = userId,
                                     IssuedOn = issuedOn,
                                     ExpiresOn = expiredOn,
                                     AuthToken = token
                                 };

            return tokenModel;
        }

        /// <summary>
        /// Method to validate token against expiry and existence in database.
        /// </summary>
        /// <param name="authToken"></param>
        /// <returns></returns>
        public bool ValidateToken(string authToken)
        {
            var token = _unitOfWork.TokenRepository.GetByID(authToken);
            if (token != null && !(DateTime.Now > token.ExpiryDate))
            {
                token.ExpiryDate = token.ExpiryDate.AddSeconds(
                                              Convert.ToDouble(20000));
                _unitOfWork.TokenRepository.Update(token);
                _unitOfWork.Save();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Method to validate token against expiry and existence in database.
        /// </summary>
        /// <param name="authToken"></param>
        /// <returns></returns>
        public bool ValidateAuthToken(string authToken)
        {
            var token = _unitOfWork.TokenRepository.GetSingle(t=>t.Value==authToken);
            if (token != null && !(DateTime.Now > token.ExpiryDate))
            {
                token.ExpiryDate = token.ExpiryDate.AddSeconds(
                                              Convert.ToDouble(2));
                _unitOfWork.TokenRepository.Update(token);
                _unitOfWork.Save();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Method to kill the provided token id.
        /// </summary>
        /// <param name="tokenId">true for successful delete</param>
        public bool Kill(string tokenId)
        {
            _unitOfWork.TokenRepository.Delete(x => x.Value == tokenId);
            _unitOfWork.Save();
            var isNotDeleted = _unitOfWork.TokenRepository.GetMany(x => x.Value == tokenId).Any();
            if (isNotDeleted) { return false; }
            return true;
        }

        /// <summary>
        /// Delete tokens for the specific deleted user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>true for successful delete</returns>
        public bool DeleteByUserId(int userId)
        {
            _unitOfWork.TokenRepository.Delete(x => x.UserFID == userId);
            _unitOfWork.Save();

            var isNotDeleted = _unitOfWork.TokenRepository.GetMany(x => x.UserFID == userId).Any();
            return !isNotDeleted;
        }

        #endregion
    }
}
