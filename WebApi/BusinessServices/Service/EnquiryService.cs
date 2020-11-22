using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using AutoMapper;
using BusinessEntities;
using DataModel;
using DataModel.UnitOfWork;
using System;
using Common.Constants;

namespace BusinessServices
{
    /// <summary>
    /// Offers services for goods specific CRUD operations
    /// </summary>
    public class EnquiryService:IEnquiryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOTPService _otpService;
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById(GenericConstant.INDIAN_STANDARD_TIME);

        /// <summary>
        /// Public constructor.
        /// </summary>
        public EnquiryService(IUnitOfWork unitOfWork,IOTPService otpService)
        {
            _unitOfWork = unitOfWork;
            _otpService = otpService;
        }

        /// <summary>
        /// Fetches goods details by id
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public BusinessEntities.EnquiryResponseEntity GetGoods(int goodsId)
        {
            var goods = _unitOfWork.EnquiryRepository.GetByID(goodsId);
            if (goods != null)
            {
                return new EnquiryResponseEntity
                {
                    EnquiryId = Convert.ToInt32(goods.EnquiryPID),
                    Name = goods.Name,
                    MobileNumber = Convert.ToString( goods.MobileNumber),
                    From = goods.From,
                    To = goods.To,
                    FromAddress = goods.FromAddress,
                    ToAddress = goods.ToAddress,
                    MinWeight = Convert.ToInt32(goods.MinWeight),
                    MaxWeight = Convert.ToInt32(goods.MaxWeight),
                    Freight = Convert.ToInt32(goods.Freight),
                    //  FreightToDisplay = "Rs " + Convert.ToInt32(goods.Freight),
                    MaterialType = goods.MaterialType.Type,
                    VehicleType = goods.VehicleType.Type,
                    ImgVehicleType = GenericConstant.IMAGES_BASE_ADDRESS + goods.VehicleType.Type.Replace(" ", "") + GenericConstant.IMG_EXT,
                    VehicleLength = goods.VehicleLength,
                    Status = goods.Status,
                    //   LastUpdated = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(goods.CreationDate), TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")),
                    ValidTill = goods.ExpiryDate,
                    // BidPID = bid != null ? bid.BidPID : 0,
                    // NoOfBids = bids != null ? bids.Count() : 0,
                    // BidPrice = bid != null ? Convert.ToInt32(bid.Price) : 0,
                    // BidPriceToDisplay = "Rs " + Convert.ToString(bid != null ? Convert.ToInt32(bid.Price) : 0),
                    //  ImagesUrl = images != null ? images.Select(im => im).ToList() : new List<string>(),
                    UserId = Convert.ToInt64(goods.UserFID)
                };
            }
            return null;
        }

        /// <summary>
        /// Fetches all the goodss.
        /// </summary>
        /// <returns></returns>
        public List<BusinessEntities.EnquiryResponseEntity> GetAllEnquiries(int userId)
        {
            var goods = GetEnquiriesByUserAccess(userId);
            
            if (goods.Any())
            {
                return MapGoods(goods);
            }
            return null;
        }

        private List<Enquiry> GetEnquiriesByUserAccess(int userId)
        {
            var userAccess = _unitOfWork.UserAccessRepository.GetMany(x => x.UserFID == userId && x.IsActive==true).Select(x=>x.Code).ToList();
            if(userAccess.Contains("AE"))
            {
                return _unitOfWork.EnquiryRepository.GetAll().OrderByDescending(x => x.CreationDate).ToList();
            }

            return _unitOfWork.EnquiryRepository.GetMany(x=>x.UserFID==userId).OrderByDescending(x => x.CreationDate).ToList(); ;
        }

        public List<BusinessEntities.EnquiryResponseEntity> GetAllEnquiriesByFilter(int userId,List<string> conditions)
        {
            var goods = GetFilteredEnquiriesByKeys(conditions,userId);

            if (goods.Any())
            {
                return MapGoods(goods);
            }
            return null;
        }
        private List<Enquiry> GetFilteredEnquiriesByKeys(List<string> conditions,int userId)
        {
            var userAccess = _unitOfWork.UserAccessRepository.GetMany(x => x.UserFID == userId && x.IsActive == true).Select(x => x.Code).ToList();
            List<Enquiry> allEnq = null;
            List<Enquiry> filteredEnq = null;
            var utcDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            var indiaDate = new DateTime(utcDate.Year, utcDate.Month, utcDate.Day, 12, 0, 0);
            if (userAccess.Contains("AE"))
            {
                allEnq = _unitOfWork.EnquiryRepository.GetAll().OrderByDescending(x => x.CreationDate).ToList();
            }
            else
            {
                allEnq = _unitOfWork.EnquiryRepository.GetMany(x => x.UserFID == userId).OrderByDescending(x => x.CreationDate).ToList();
            }

            foreach (var condition in conditions)
            {
                if (condition.Equals("L1M", StringComparison.InvariantCultureIgnoreCase))
                    filteredEnq = allEnq.Where(x => x.CreationDate >= indiaDate.AddDays(-30)).ToList();

                if (condition.Equals("L7D", StringComparison.InvariantCultureIgnoreCase))
                    filteredEnq = allEnq.Where(x => x.CreationDate >= indiaDate.AddDays(-7)).ToList();

                if (condition.Equals("TD", StringComparison.InvariantCultureIgnoreCase))
                    filteredEnq = allEnq.Where(x => x.CreationDate >= indiaDate).ToList();

                if (filteredEnq != null)
                {
                    var filEq = filteredEnq.Select(x => x.EnquiryPID).ToList();
                    if (condition.Equals("F1M", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var fe = allEnq.Where(x => x.ExpiryDate >= indiaDate.AddDays(+30)).ToList();
                        filteredEnq.AddRange(fe.Where(x => !filEq.Contains(x.EnquiryPID)).ToList());
                    }

                    if (condition.Equals("F7D", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var fe = allEnq.Where(x => x.ExpiryDate >= indiaDate.AddDays(+7)).ToList();
                        filteredEnq.AddRange(fe.Where(x => !filEq.Contains(x.EnquiryPID)).ToList());
                    }

                    if (condition.Equals("MTD", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var fe = allEnq.Where(x => x.ExpiryDate >= indiaDate).ToList();
                        filteredEnq.AddRange(fe.Where(x => !filEq.Contains(x.EnquiryPID)).ToList());
                    }
                }
            }

            return filteredEnq;
        }

        public List<BusinessEntities.EnquiryResponseEntity> GetAllEnquiries()
        {
            var goods = _unitOfWork.EnquiryRepository.GetAll().OrderByDescending(x => x.CreationDate).ToList();
            if (goods.Any())
            {
                return MapGoods(goods);
            }
            return null;
        }


        /// <summary>
        /// Fetches all the goodss.
        /// </summary>
        /// <returns></returns>
        public List<BusinessEntities.GoodsResponseEntity> GetGoodsByUser(UserEntity userEntity)
        {
            var goods = _unitOfWork.GoodsRepository.GetMany(x=>x.UserFID==userEntity.UserId).ToList();
            if (goods.Any())
            {
                return null;// MapGoods(goods);
                //Mapper.CreateMap<Good, GoodsRequestEntity>();
                //var goodsModel = Mapper.Map<List<Good>, List<GoodsRequestEntity>>(goods);
                //return goodsModel;
            }
            return null;
        }

        /// <summary>
        /// Creates a goods
        /// </summary>
        /// <param name="goodsEntity"></param>
        /// <returns></returns>
        public long CreateEnquiry(BusinessEntities.EnquiryRequestEntity goodsEntity)
        {
            using (var scope = new TransactionScope())
            {
                //int? nullable = null;
                DateTime ed = goodsEntity.ValidTill != DateTime.MinValue ? TimeZoneInfo.ConvertTimeFromUtc(goodsEntity.ValidTill.ToUniversalTime(), INDIAN_ZONE): TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                var expiryDate = new DateTime(ed.Year, ed.Month, ed.Day, 0, 0, 0);
                long? userId = null;
                var goods = new Enquiry
                {
                    Name = goodsEntity.Name,
                    From = goodsEntity.From,
                    To = goodsEntity.To,
                    FromAddress = goodsEntity.FromAddress,
                    ToAddress = goodsEntity.ToAddress,
                    ExpiryDate = expiryDate,// TimeZoneInfo.ConvertTimeFromUtc(goodsEntity.ValidTill.ToUniversalTime(), INDIAN_ZONE),
                    CreationDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE),
                    LastUpdated = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE),
                    MaxWeight = goodsEntity.MaxWeight,
                    VehicleTypeFID = goodsEntity.VehicleType,
                    VehicleLength = goodsEntity.VehicleLength,
                    MaterialTypeFID = goodsEntity.MaterialType,
                    Status = goodsEntity.Status,
                    UserFID = goodsEntity.UserId != 0 ? goodsEntity.UserId : userId,
                    Comments="",
                };

                if (!string.IsNullOrEmpty(goodsEntity.MobileNumber))
                    goods.MobileNumber = Convert.ToInt64(goodsEntity.MobileNumber);

                if (!string.IsNullOrEmpty(goodsEntity.Comments))
                    goods.Comments = goodsEntity.Comments;

                    _unitOfWork.EnquiryRepository.Insert(goods);
                _unitOfWork.Save();
                scope.Complete();
                return goods.EnquiryPID;
            }
        }

        /// <summary>
        /// Updates a goods
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="goodsEntity"></param>
        /// <returns></returns>
        public bool UpdateGoods(int goodsId, BusinessEntities.EnquiryRequestEntity goodsEntity)
        {
            var success = false;
            if (goodsEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    
                    var goods = _unitOfWork.EnquiryRepository.GetByID(goodsId);
                   
                    if (goods != null)
                    {
                        DateTime ed = TimeZoneInfo.ConvertTimeFromUtc(goodsEntity.ValidTill.ToUniversalTime(), INDIAN_ZONE);
                        var expiryDate = new DateTime(ed.Year, ed.Month, ed.Day, 0, 0, 0);
                        goods.From = goodsEntity.From;
                        goods.To = goodsEntity.To;
                        goods.FromAddress = goodsEntity.FromAddress;
                        goods.ToAddress = goodsEntity.ToAddress;
                        goods.Name = goodsEntity.Name;
                        goods.MobileNumber = !string.IsNullOrEmpty(goodsEntity.MobileNumber) ? Convert.ToInt64(goodsEntity.MobileNumber) : 0;
                        goods.Freight = goodsEntity.Freight;
                        goods.ExpiryDate = expiryDate;
                        goods.MaxWeight = goodsEntity.MaxWeight;
                        goods.VehicleTypeFID = goodsEntity.VehicleType;
                        goods.VehicleLength = goodsEntity.VehicleLength;
                        goods.MaterialTypeFID = goodsEntity.MaterialType;
                        goods.Status = goodsEntity.Status;
                        goods.LastUpdated = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                        if(!string.IsNullOrEmpty(goodsEntity.Comments))
                        goods.Comments = goodsEntity.Comments;

                        if (goodsEntity.UserId !=0)
                            goods.UserFID = goodsEntity.UserId;

                        _unitOfWork.EnquiryRepository.Update(goods);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        /// <summary>
        /// Deletes a particular goods
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public bool DeleteGoods(int goodsId)
        {
            var success = false;
            if (goodsId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var goods = _unitOfWork.EnquiryRepository.GetByID(goodsId);
                    var quotations = _unitOfWork.QuotationRepository.GetMany(x=>x.EnquiryFID==goodsId);
                    var bookings = _unitOfWork.BookingRepository.GetMany(x => x.EnquiryFID == goodsId);
                    if (goods != null)
                    {
                        foreach (var item in quotations)
                        {
                            _unitOfWork.QuotationRepository.Delete(item);
                        }

                        foreach (var item in bookings)
                        {
                            _unitOfWork.BookingRepository.Delete(item);
                        }

                        _unitOfWork.EnquiryRepository.Delete(goods);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        private List<EnquiryResponseEntity> MapGoods(List<Enquiry> goods)
        {
            List<EnquiryResponseEntity> listGoodsEntity  = new List<EnquiryResponseEntity>();
          
            if (goods != null && goods.Count() > 0)
            {
                goods = goods.OrderByDescending(x => x.CreationDate).ToList();

                listGoodsEntity.AddRange(goods.Select(x =>
                {
                    return new EnquiryResponseEntity
                    {
                        EnquiryId = Convert.ToInt32(x.EnquiryPID),
                        From = x.From,
                        To = x.To,
                        FromAddress = x.FromAddress,
                        ToAddress = x.ToAddress,
                        Name = x.Name,
                        MobileNumber = Convert.ToString(x.MobileNumber),
                        MinWeight = Convert.ToInt32(x.MinWeight),
                        MaxWeight = Convert.ToInt32(x.MaxWeight),
                        Freight = Convert.ToInt32(x.Freight),
                        MaterialType = x.MaterialType.Type,
                        VehicleType = x.VehicleType.Type,
                        CreationDate = x.CreationDate.ToLongDateString(),
                        //ImgVehicleType = GenericConstant.IMAGES_BASE_ADDRESS + x.VehicleType.Type.Replace(" ", "") + GenericConstant.IMG_EXT,
                        ImgVehicleType = x.VehicleType.Type.ToLower().Replace(" ", ""),
                        VehicleLength = x.VehicleLength,
                        Status = x.Status,
                        ValidTill = x.ExpiryDate,
                        UserId= Convert.ToInt64(x.UserFID),
                        Comments =  x.Comments
                    };
                }
                ));
            }
            return listGoodsEntity;
        }
    }
}
