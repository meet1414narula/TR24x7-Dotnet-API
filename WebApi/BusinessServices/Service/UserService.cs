using BusinessEntities;
using Commom.ErrorHelpers;
using Common.Constants;
using DataModel;
using DataModel.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace BusinessServices
{
    /// <summary>
    /// Offers services for user specific operations
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOTPService _otpService;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public UserService(IUnitOfWork unitOfWork,IOTPService otpService)
        {
            _unitOfWork = unitOfWork;
            _otpService = otpService;
        }

        /// <summary>
        /// Public method to authenticate user by user name and password.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public long Authenticate(long mobileNumber, string password)
        {
            var user = _unitOfWork.UserRepository.Get(u => u.MobileNumber == mobileNumber && u.Password == password);
            if (user != null && user.UserPID > 0)
            {
                return user.UserPID;
            }
            return 0;
        }

        /// <summary>
        /// Public method to authenticate user by user name and password.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public long GetUserId(long mobileNumber)
        {
            var user = _unitOfWork.UserRepository.Get(u => u.MobileNumber == mobileNumber);
            if (user != null && user.UserPID > 0)
            {
                return user.UserPID;
            }
            return 0;
        }

        public bool CheckUserExist(RegisterEntity registerEntity)
        {
            var userData = _unitOfWork.UserRepository.Get(x => x.MobileNumber ==  Convert.ToInt64(registerEntity.MobileNumber));
            if (userData != null)
            {
                throw new ApiBusinessException(1000,"Mobile number already exist.Please try with some different number.");
            }
            else
            {
               return true;
            }
        }

        public long Create(RegisterEntity registerEntity)
        {
            _otpService.Generate(Convert.ToInt64(registerEntity.MobileNumber));
            var userTypeFID = _unitOfWork.UserTypeRepository.GetSingle(x => x.Type == registerEntity.UserType).UserTypePID;
            using (var scope = new TransactionScope())
            {
                var user = new User
                {
                    MobileNumber=Convert.ToInt64(registerEntity.MobileNumber),
                    Password=registerEntity.Password,
                    ConfirmPassword=registerEntity.ConfirmPassword,
                    UserTypeFID=userTypeFID
                };
                _unitOfWork.UserRepository.Insert(user);
                _unitOfWork.Save();
                scope.Complete();
                return user.UserPID;
            }
        }
    }
}
