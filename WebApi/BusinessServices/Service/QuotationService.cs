﻿using System.Collections.Generic;
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
    public class QuotationService:IQuotationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEnquiryService _otpService;
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById(GenericConstant.INDIAN_STANDARD_TIME);

        /// <summary>
        /// Public constructor.
        /// </summary>
        public QuotationService(IUnitOfWork unitOfWork,IEnquiryService otpService)
        {
            _unitOfWork = unitOfWork;
            _otpService = otpService;
        }

        /// <summary>
        /// Fetches goods details by id
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public BusinessEntities.QuotationResponseEntity GetGoods(int goodsId)
        {
            var goods = _unitOfWork.EnquiryRepository.GetByID(goodsId);
            if (goods != null)
            {
                return new QuotationResponseEntity
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
        public List<BusinessEntities.QuotationResponseEntity> GetAllEnquiries()
        {
            var enquiries = _unitOfWork.EnquiryRepository.GetAll().OrderByDescending(x=>x.CreationDate).ToList();
            var quotations = _unitOfWork.QuotationRepository.GetMany(x=>x.Freight !=null && x.Freight !=0).OrderByDescending(x => x.CreationDate).ToList();
            if (enquiries.Any() && quotations.Any())
            {
                return MapQuotations(enquiries,quotations);
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
        public long CreateQuotation(BusinessEntities.QuotationRequestEntity goodsEntity)
        {
            Quotation goods = null;
           // _unitOfWork.Save();
            using (var scope = new TransactionScope())
            {
              
                DateTime ed = TimeZoneInfo.ConvertTimeFromUtc(goodsEntity.ValidTill.ToUniversalTime(), INDIAN_ZONE);
                var expiryDate = new DateTime(ed.Year, ed.Month, ed.Day, 0, 0, 0);

                goods = _unitOfWork.QuotationRepository.GetByID(goodsEntity.EnquiryId);
                if (goods == null)
                {
                    goods = new Quotation();
                }

                goods.EnquiryFID = goodsEntity.EnquiryId;
                goods.ExpiryDate = expiryDate;// TimeZoneInfo.ConvertTimeFromUtc(goodsEntity.ValidTill.ToUniversalTime(), INDIAN_ZONE),
                goods.CreationDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                goods.LastUpdated = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                goods.IsActive = true;
                goods.Status = goodsEntity.Status;

                if (goodsEntity.Freight != 0)
                {
                    goods.Freight = goodsEntity.Freight;
                }

                if (!string.IsNullOrEmpty(goodsEntity.LoadingCharges))
                {
                    goods.LoadingCharges = goodsEntity.LoadingCharges;
                }

                if (!string.IsNullOrEmpty(goodsEntity.UnloadingCharges))
                {
                    goods.UnloadingCharges = goodsEntity.UnloadingCharges;
                }

                if (!string.IsNullOrEmpty(goodsEntity.PackagingCharges))
                {
                    goods.PackagingCharges = goodsEntity.PackagingCharges;
                }

                _unitOfWork.QuotationRepository.Insert(goods);

                //update enquiry as well
                var enq = _unitOfWork.EnquiryRepository.GetByID(goodsEntity.EnquiryId);
                _unitOfWork.EnquiryRepository.Update(enq);
                enq.Status = goodsEntity.Status;



                _unitOfWork.Save();
                scope.Complete();
                return goods.QuotationPID;
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

        private List<QuotationResponseEntity> MapQuotations(List<Enquiry> enquiries, List<Quotation> quotations)
        {
            List<QuotationResponseEntity> listGoodsEntity  = new List<QuotationResponseEntity>();
          
            if (quotations != null && quotations.Count() > 0)
            {
                quotations = quotations.OrderByDescending(x => x.CreationDate).ToList();

                listGoodsEntity.AddRange(quotations.Select(qt =>
                {
                    var enquiry = enquiries.Where(y => y.EnquiryPID == qt.EnquiryFID).FirstOrDefault();
                    if (enquiry != null)
                    {
                        return new QuotationResponseEntity
                        {
                            EnquiryId = Convert.ToInt32(enquiry.EnquiryPID),
                            LoadingCharges = qt.LoadingCharges,
                            UnloadingCharges = qt.UnloadingCharges,
                            PackagingCharges = qt.PackagingCharges,
                            Freight = Convert.ToInt32(qt.Freight),
                            From = enquiry.From,
                            To = enquiry.To,
                            FromAddress = string.IsNullOrEmpty(enquiry.FromAddress) ? enquiry.From:enquiry.FromAddress ,
                            ToAddress = string.IsNullOrEmpty(enquiry.ToAddress) ? enquiry.To : enquiry.ToAddress,
                            Name = enquiry.Name,
                            MobileNumber = Convert.ToString(enquiry.MobileNumber),
                            MinWeight = Convert.ToInt32(enquiry.MinWeight),
                            MaxWeight = Convert.ToInt32(enquiry.MaxWeight),
                            MaterialType = enquiry.MaterialType.Type,
                            VehicleType = enquiry.VehicleType.Type,
                            //ImgVehicleType = GenericConstant.IMAGES_BASE_ADDRESS + x.VehicleType.Type.Replace(" ", "") + GenericConstant.IMG_EXT,
                            ImgVehicleType = enquiry.VehicleType.Type.ToLower().Replace(" ", ""),
                            VehicleLength = enquiry.VehicleLength,
                            Status = enquiry.Status,
                            ValidTill = enquiry.ExpiryDate,
                            UserId = Convert.ToInt64(enquiry.UserFID)
                        };
                    }
                    else
                    {
                        return new QuotationResponseEntity
                        {
                            EnquiryId = Convert.ToInt32(enquiry.EnquiryPID),
                            From = enquiry.From,
                            To = enquiry.To,
                            FromAddress = enquiry.FromAddress,
                            ToAddress = enquiry.ToAddress,
                            Name = enquiry.Name,
                            MobileNumber = Convert.ToString(enquiry.MobileNumber),
                            MinWeight = Convert.ToInt32(enquiry.MinWeight),
                            MaxWeight = Convert.ToInt32(enquiry.MaxWeight),
                            Freight = Convert.ToInt32(enquiry.Freight),
                            MaterialType = enquiry.MaterialType.Type,
                            VehicleType = enquiry.VehicleType.Type,
                            //ImgVehicleType = GenericConstant.IMAGES_BASE_ADDRESS + x.VehicleType.Type.Replace(" ", "") + GenericConstant.IMG_EXT,
                            ImgVehicleType = enquiry.VehicleType.Type.ToLower().Replace(" ", ""),
                            VehicleLength = enquiry.VehicleLength,
                            Status = enquiry.Status,
                            ValidTill = enquiry.ExpiryDate,
                            UserId = Convert.ToInt64(enquiry.UserFID)
                        };
                    }
                }
                ));
            }
            return listGoodsEntity;
        }
    }
}
