using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using AutoMapper;
using BusinessEntities;
using DataModel;
using DataModel.UnitOfWork;
using System;
using Common.Constants;
using System.Configuration;

namespace BusinessServices
{
    /// <summary>
    /// Offers services for goods specific CRUD operations
    /// </summary>
    public class HelperService:IHelperService
    {
        #region Private member variables.
        private readonly IUnitOfWork _unitOfWork;
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById(GenericConstant.INDIAN_STANDARD_TIME);
        public StaticDataEntity staticDataEntity = null;
        #endregion

        #region Public constructor.
        /// <summary>
        /// Public constructor.
        /// </summary>
        public HelperService(IUnitOfWork unitOfWork)
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
        public StaticDataEntity GetStaticData()
        {
            staticDataEntity = new StaticDataEntity();
            var vehicleTypes = _unitOfWork.VehicleTypeRepository.GetAll().ToList();
            var enquiryStatus = _unitOfWork.EnquiryStatusRepository.GetAll().ToList();
            var userTypes = _unitOfWork.UserTypeRepository.GetAll().ToList();
          //  var userProfessions = _unitOfWork.GenericRepository<UserProfession>().GetAll().ToList();
           // var vehicleCompanies = _unitOfWork.GenericRepository<VehicleCompany>().GetAll().ToList();
            var materialTypes = _unitOfWork.MaterialTypeRepository.GetAll().ToList();
            var configKeyValue = _unitOfWork.ConfigKeyValueRepository.GetAll().ToList();
            var cities = _unitOfWork.CityRepository.GetMany(x => x.IsActive == true).ToList();
            var maxWeight = configKeyValue.Where(x => x.Key == GenericConstant.CONFIGKEYS.MAX_WEIGHT).FirstOrDefault();
            var maxLength = configKeyValue.Where(x => x.Key == GenericConstant.CONFIGKEYS.MAX_LENGTH).FirstOrDefault();
            var minLength = configKeyValue.Where(x => x.Key == GenericConstant.CONFIGKEYS.MIN_LENGTH).FirstOrDefault();

            staticDataEntity.MaxWeights = new List<int>();
            staticDataEntity.MaxWeights = Enumerable.Range(1, Convert.ToInt32(maxWeight.Value)).ToList();

            staticDataEntity.VehicleLengths = new List<string>();
            for (int i = Convert.ToInt32(minLength.Value); i <= Convert.ToInt32(maxLength.Value); i++)
            {
                staticDataEntity.VehicleLengths.Add(i + GenericConstant.FEET);
            }

            staticDataEntity.Cities = new List<CityEntity>();
            staticDataEntity.Cities.AddRange(cities.OrderBy(x => x.Name).Select(x =>
            {
                return new CityEntity
                {
                    Id = x.CityPID,
                    ActualName = x.Name,
                    Name = x.Name + string.Format(GenericConstant.STATE_CODE, x.StateCode),
                    State = x.State,
                    IsActive = x.IsActive
                };
            }
            ));

            staticDataEntity.States = new List<CityEntity>();
            staticDataEntity.States.AddRange(cities.OrderBy(x => x.State).GroupBy(x => x.State).Select(x =>
            {
                return new CityEntity
                {
                    Id = x.First().CityPID,
                    ActualName = x.First().State,
                    Name = x.First().State
                };
            }
            ));


            if (vehicleTypes != null && vehicleTypes.Count > 0)
            {
                staticDataEntity.VehicleTypes = new List<VehicleTypeEntity>();
                staticDataEntity.VehicleTypes.AddRange(vehicleTypes.OrderBy(x => x.Type).Select(x =>
                {
                    return new VehicleTypeEntity
                    {
                        Id = x.VehicleTypePID,
                        Type = x.Type
                    };
                }
                ));
            }

            if (enquiryStatus != null && enquiryStatus.Count > 0)
            {
                staticDataEntity.EnquiryStatus = new List<EnquiryStatusEntity>();
                staticDataEntity.EnquiryStatus.AddRange(enquiryStatus.OrderBy(x => x.DisplayOrder).Select(x =>
                {
                    return new EnquiryStatusEntity
                    {
                        Id = x.EnquiryStatusPID,
                        Status = x.Status
                    };
                }
                ));
            }

            if (userTypes != null && userTypes.Count > 0)
            {
                staticDataEntity.UserTypes = new List<UserTypeEntity>();
                staticDataEntity.UserTypes.AddRange(userTypes.OrderBy(x => x.Type).Select(x =>
                {
                    return new UserTypeEntity
                    {
                        Id = x.UserTypePID,
                        Type = x.Type
                    };
                }
                ));
            }

          
            //if (vehicleCompanies != null && vehicleCompanies.Count > 0)
            //{
            //    staticDataEntity.VehicleCompanies = new List<VehCompany>();
            //    staticDataEntity.VehicleCompanies.AddRange(vehicleCompanies.OrderBy(x => x.Name).Select(x =>
            //    {
            //        return new VehCompany
            //        {
            //            Id = x.VehicleCompanyPID,
            //            Name = x.Name
            //        };
            //    }
            //    ));
            //}

            if (materialTypes != null && materialTypes.Count > 0)
            {
                staticDataEntity.MaterialTypes = new List<MaterialTypeEntity>();
                staticDataEntity.MaterialTypes.AddRange(materialTypes.OrderBy(x => x.Type).Select(x =>
                {
                    return new MaterialTypeEntity
                    {
                        Id = Convert.ToInt32(x.MaterialTypePID),
                        Type = x.Type
                    };
                }
                ));
            }
            staticDataEntity.SupportNumber = ConfigurationManager.AppSettings[GenericConstant.SUPPORT_NUMBER];

            return staticDataEntity;
        }

        #endregion
    }
}
