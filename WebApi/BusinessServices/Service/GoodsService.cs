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
    public class GoodsService:IGoodsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOTPService _otpService;
      
        /// <summary>
        /// Public constructor.
        /// </summary>
        public GoodsService(IUnitOfWork unitOfWork,IOTPService otpService)
        {
            _unitOfWork = unitOfWork;
            _otpService = otpService;
        }

        /// <summary>
        /// Fetches goods details by id
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public BusinessEntities.GoodsResponseEntity GetGoods(int goodsId)
        {
            var goods = _unitOfWork.GoodsRepository.GetByID(goodsId);
            if (goods != null)
            {
                return new GoodsResponseEntity
                {
                    GoodsId = Convert.ToInt32(goods.GoodsPID),
                    From = goods.From,
                    To = goods.To,
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
                    ValidTill = Convert.ToString(goods.ExpiryDate),
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
        public List<BusinessEntities.GoodsResponseEntity> GetAllGoods()
        {
            var goods = _unitOfWork.GoodsRepository.GetAll().ToList();
            if (goods.Any())
            {
                return MapGoods(goods);
                //Mapper.CreateMap<Good, GoodsRequestEntity>();
                //var goodsModel = Mapper.Map<List<Good>, List<GoodsRequestEntity>>(goods);
                //return goodsModel;
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
                return MapGoods(goods);
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
        public long CreateGoods(BusinessEntities.GoodsRequestEntity goodsEntity)
        {
            if(goodsEntity.UserId == 0)
            _otpService.Verify(goodsEntity.OTP);

            using (var scope = new TransactionScope())
            {

                //this.goodsEntity.from = this.goodsForm.value.from;
                //this.goodsEntity.to = this.goodsForm.value.to;
                //this.goodsEntity.freight = this.goodsForm.value.freight;
                //this.goodsEntity.validTill = this.goodsForm.value.validTill;
                //this.goodsEntity.maxWeight = this.goodsForm.value.maxWeight;
                //this.goodsEntity.vehicleType = this.goodsForm.value.vehicleType;
                //this.goodsEntity.vehicleLength = this.goodsForm.value.vehicleLength;
                //this.goodsEntity.materialType = this.goodsForm.value.materialType;
                //this.goodsEntity.status = this.goodsForm.value.status;
                var goods = new Good
                {
                    From = goodsEntity.From,
                    To=goodsEntity.To,
                    MobileNumber= !string.IsNullOrEmpty(goodsEntity.MobileNumber) ? Convert.ToInt64(goodsEntity.MobileNumber):0,
                    Freight = goodsEntity.Freight,
                    ExpiryDate= goodsEntity.ValidTill,
                    MaxWeight=goodsEntity.MaxWeight,
                    VehicleTypeFID=goodsEntity.VehicleType,
                    VehicleLength=goodsEntity.VehicleLength,
                    MaterialTypeFID=goodsEntity.MaterialType,
                    Status=goodsEntity.Status,
                    UserFID=goodsEntity.UserId
                };

                if(string.IsNullOrEmpty(goodsEntity.LoadingCharges))
                {
                    goods.LoadingCharges = goodsEntity.LoadingCharges;
                }

                if (string.IsNullOrEmpty(goodsEntity.UnloadingCharges))
                {
                    goods.UnloadingCharges = goodsEntity.UnloadingCharges;
                }

                if (string.IsNullOrEmpty(goodsEntity.PackagingCharges))
                {
                    goods.PackagingCharges = goodsEntity.PackagingCharges;
                }

                _unitOfWork.GoodsRepository.Insert(goods);
                _unitOfWork.Save();
                scope.Complete();
                return goods.GoodsPID;
            }
        }

        /// <summary>
        /// Updates a goods
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="goodsEntity"></param>
        /// <returns></returns>
        public bool UpdateGoods(int goodsId, BusinessEntities.GoodsRequestEntity goodsEntity)
        {
            var success = false;
            if (goodsEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var goods = _unitOfWork.GoodsRepository.GetByID(goodsId);
                    if (goods != null)
                    {
                        goods.From = goodsEntity.From;
                        goods.To = goodsEntity.To;
                        goods.MobileNumber = !string.IsNullOrEmpty(goodsEntity.MobileNumber) ? Convert.ToInt64(goodsEntity.MobileNumber) : 0;
                        goods.Freight = goodsEntity.Freight;
                        goods.ExpiryDate = goodsEntity.ValidTill;
                        goods.MaxWeight = goodsEntity.MaxWeight;
                        goods.VehicleTypeFID = goodsEntity.VehicleType;
                        goods.VehicleLength = goodsEntity.VehicleLength;
                        goods.MaterialTypeFID = goodsEntity.MaterialType;
                        goods.Status = goodsEntity.Status;
                        _unitOfWork.GoodsRepository.Update(goods);
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
                    var goods = _unitOfWork.GoodsRepository.GetByID(goodsId);
                    if (goods != null)
                    {

                        _unitOfWork.GoodsRepository.Delete(goods);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }



        private List<GoodsResponseEntity> MapGoods(List<Good> goods)
        {
            List<GoodsResponseEntity> listGoodsEntity  = new List<GoodsResponseEntity>();
          
            if (goods != null && goods.Count() > 0)
            {
                goods = goods.OrderByDescending(x => x.CreationDate).ToList();

                listGoodsEntity.AddRange(goods.Select(x =>
                {
                     var bids = x.Bids.Where(y => y.IsActive == true).OrderByDescending(y => y.LastUpdatedDate).ToList();
                     var bid = bids != null ? bids.FirstOrDefault() : null;
                  //   var address = x.User.Addresses.OrderByDescending(y => y.LastUpdated).FirstOrDefault();
                 //   var company = x.User.Companies.OrderByDescending(y => y.LastUpdated).FirstOrDefault();
                   // var images = x.Images != null ? x.Images.OrderByDescending(y => y.LastUpdated).Select(y => y.URL).ToList() : null;
                    return new GoodsResponseEntity
                    {
                        GoodsId = Convert.ToInt32(x.GoodsPID),
                        From = x.From,
                        To = x.To,
                        MinWeight = Convert.ToInt32(x.MinWeight),
                        MaxWeight = Convert.ToInt32(x.MaxWeight),
                        Freight = Convert.ToInt32(x.Freight),
                      //  FreightToDisplay = "Rs " + Convert.ToInt32(x.Freight),
                        MaterialType = x.MaterialType.Type,
                        VehicleType = x.VehicleType.Type,
                        ImgVehicleType = GenericConstant.IMAGES_BASE_ADDRESS + x.VehicleType.Type.Replace(" ", "") + GenericConstant.IMG_EXT,
                        VehicleLength = x.VehicleLength,
                        Status = x.Status,
                     //   LastUpdated = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(x.CreationDate), TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")),
                        ValidTill = Convert.ToString(x.ExpiryDate),
                       // BidPID = bid != null ? bid.BidPID : 0,
                        TotalBidsCount = bids != null ? bids.Count() : 0,
                        CurrentBidPrice = bid != null ? Convert.ToInt32(bid.Price) : Convert.ToInt32(x.Freight),
                       // BidPriceToDisplay = "Rs " + Convert.ToString(bid != null ? Convert.ToInt32(bid.Price) : 0),
                      //  ImagesUrl = images != null ? images.Select(im => im).ToList() : new List<string>(),
                       UserId= Convert.ToInt64(x.UserFID)
                    };
                }
                ));
            }
            return listGoodsEntity;
        }
    }
}
