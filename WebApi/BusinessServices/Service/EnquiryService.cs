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
        public List<BusinessEntities.EnquiryResponseEntity> GetAllEnquiries()
        {
            var goods = _unitOfWork.EnquiryRepository.GetAll().OrderByDescending(x=>x.CreationDate).ToList();
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
                DateTime ed = TimeZoneInfo.ConvertTimeFromUtc(goodsEntity.ValidTill.ToUniversalTime(), INDIAN_ZONE);
                var expiryDate = new DateTime(ed.Year, ed.Month, ed.Day, 0, 0, 0);
                long? userId = null;
                var goods = new Enquiry
                {
                    Name = goodsEntity.Name,
                    From = goodsEntity.From,
                    To = goodsEntity.To,
                    FromAddress = goodsEntity.FromAddress,
                    ToAddress = goodsEntity.ToAddress,
                    MobileNumber = !string.IsNullOrEmpty(goodsEntity.MobileNumber) ? Convert.ToInt64(goodsEntity.MobileNumber) : 0,
                    ExpiryDate = expiryDate,// TimeZoneInfo.ConvertTimeFromUtc(goodsEntity.ValidTill.ToUniversalTime(), INDIAN_ZONE),
                    CreationDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE),
                    LastUpdated = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE),
                    MaxWeight = goodsEntity.MaxWeight,
                    VehicleTypeFID = goodsEntity.VehicleType,
                    VehicleLength = goodsEntity.VehicleLength,
                    MaterialTypeFID = goodsEntity.MaterialType,
                    Status = goodsEntity.Status,
                    UserFID = goodsEntity.UserId != 0 ? goodsEntity.UserId : userId
                };
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
                    if (goods != null)
                    {

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
                        //ImgVehicleType = GenericConstant.IMAGES_BASE_ADDRESS + x.VehicleType.Type.Replace(" ", "") + GenericConstant.IMG_EXT,
                        ImgVehicleType = x.VehicleType.Type.ToLower().Replace(" ", ""),
                        VehicleLength = x.VehicleLength,
                        Status = x.Status,
                        ValidTill = x.ExpiryDate,
                        UserId= Convert.ToInt64(x.UserFID)
                    };
                }
                ));
            }
            return listGoodsEntity;
        }
    }
}
