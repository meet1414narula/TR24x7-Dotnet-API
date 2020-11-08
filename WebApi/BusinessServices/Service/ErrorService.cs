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
using BusinessEntities;

namespace BusinessServices
{
    public class ErrorService : IErrorService
    {
        #region Private member variables.
        private readonly IUnitOfWork _unitOfWork;
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById(GenericConstant.INDIAN_STANDARD_TIME);
        
        #endregion

        #region Public constructor.

        /// <summary>
        /// Public constructor.
        /// </summary>
        public ErrorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public member methods.

        public void AddError(ErrorEntity errorEntity)
        {
            var error = new Error
            {
                CreationDate= TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE),
                Message=errorEntity.Message,
                StackTrace=errorEntity.StackTrace,
                TargetSite=errorEntity.TargetSite
            };
            _unitOfWork.ErrorRepository.Insert(error);
        }

      

        #endregion

        
    }
}
