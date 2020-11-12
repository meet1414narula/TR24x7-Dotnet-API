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
    public class BookingService:IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEnquiryService _otpService;
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById(GenericConstant.INDIAN_STANDARD_TIME);

        /// <summary>
        /// Public constructor.
        /// </summary>
        public BookingService(IUnitOfWork unitOfWork,IEnquiryService otpService)
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
        public List<BusinessEntities.BookingResponseEntity> GetAllQuotations()
        {
            var enquiries = _unitOfWork.EnquiryRepository.GetAll().OrderByDescending(x=>x.CreationDate).ToList();
            var quotations = _unitOfWork.BookingRepository.GetMany(x=>x.Freight !=null && x.Freight !=0).OrderByDescending(x => x.CreationDate).ToList();
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
        public List<BusinessEntities.BookingResponseEntity> GetAllQuotes(QuoteRequestEntity quotationRequestEntity)
        {
            var enquiries = _unitOfWork.EnquiryRepository.GetMany(x=>x.From.Equals(quotationRequestEntity.From,StringComparison.InvariantCultureIgnoreCase)  && x.To.Equals(quotationRequestEntity.To, StringComparison.InvariantCultureIgnoreCase)).OrderByDescending(x => x.CreationDate).ToList();
            if(!string.IsNullOrEmpty(quotationRequestEntity.VehicleLength))
            {
                enquiries = enquiries.Where(x => x.VehicleLength.Equals(quotationRequestEntity.VehicleLength, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            var enquiryIds = enquiries.Select(x => x.EnquiryPID).ToList();
            var quotations = _unitOfWork.BookingRepository.GetMany(x=> enquiryIds.Contains(Convert.ToInt64(x.EnquiryFID))).OrderByDescending(x => x.CreationDate).ToList();
            if (enquiries.Any() && quotations.Any())
            {
                return MapQuotations(enquiries, quotations);
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
        public long CreateQuotation(BusinessEntities.BookingRequestEntity goodsEntity)
        {
            Booking goods = null;
            bool quotationFound = false;
           // _unitOfWork.Save();
            using (var scope = new TransactionScope())
            {
              
                DateTime ed = TimeZoneInfo.ConvertTimeFromUtc(goodsEntity.ValidTill.ToUniversalTime(), INDIAN_ZONE);
                var expiryDate = new DateTime(ed.Year, ed.Month, ed.Day, 0, 0, 0);

                goods = _unitOfWork.BookingRepository.GetMany(x=>x.EnquiryFID == goodsEntity.EnquiryId).FirstOrDefault();
                if (goods == null)
                {
                    goods = new Booking();
                }
                else
                {
                    quotationFound = true;
                }

                goods.EnquiryFID = goodsEntity.EnquiryId;
                goods.DOM = expiryDate;// TimeZoneInfo.ConvertTimeFromUtc(goodsEntity.ValidTill.ToUniversalTime(), INDIAN_ZONE),
                goods.CreationDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                goods.LastUpdated = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                goods.IsActive = true;
                goods.Status = goodsEntity.Status;

                if (goodsEntity.Freight != 0)
                {
                    goods.Freight = goodsEntity.Freight;
                }

                if (goodsEntity.Advance != 0)
                {
                    goods.Advance = goodsEntity.Advance;
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

                if (quotationFound)
                {
                    _unitOfWork.BookingRepository.Update(goods);
                }
                else
                {
                    _unitOfWork.BookingRepository.Insert(goods);
                }

                //update enquiry as well
                var enq = _unitOfWork.EnquiryRepository.GetByID(goodsEntity.EnquiryId);
                enq.Status = goodsEntity.Status;
                _unitOfWork.EnquiryRepository.Update(enq);

                _unitOfWork.Save();
                scope.Complete();
                return goods.BookingPID;
            }
        }

        /// <summary>
        /// Updates a goods
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="goodsEntity"></param>
        /// <returns></returns>
        public bool UpdateGoods(int goodsId, BusinessEntities.BookingRequestEntity goodsEntity)
        {
            var success = false;
            if (goodsEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    
                    var goods = _unitOfWork.BookingRepository.GetFirst(x=>x.EnquiryFID==goodsId);
                   
                    if (goods != null)
                    {
                        DateTime ed = TimeZoneInfo.ConvertTimeFromUtc(goodsEntity.ValidTill.ToUniversalTime(), INDIAN_ZONE);
                        var expiryDate = new DateTime(ed.Year, ed.Month, ed.Day, 0, 0, 0);
                        goods.Freight = goodsEntity.Freight;
                        goods.DOM = expiryDate;

                        if (goodsEntity.Freight != 0)
                        {
                            goods.Freight = goodsEntity.Freight;
                        }

                        if (goodsEntity.Advance != 0)
                        {
                            goods.Advance = goodsEntity.Advance;
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


                        goods.Status = goodsEntity.Status;
                        goods.LastUpdated = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                        _unitOfWork.BookingRepository.Update(goods);
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
                    var goods = _unitOfWork.BookingRepository.GetMany(x=>x.EnquiryFID==goodsId);
                    if (goods != null)
                    {
                        foreach (var item in goods)
                        {
                            _unitOfWork.BookingRepository.Delete(item);
                        }
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        private List<BookingResponseEntity> MapQuotations(List<Enquiry> enquiries, List<Booking> bookings)
        {
            List<BookingResponseEntity> listGoodsEntity  = new List<BookingResponseEntity>();
          
            if (bookings != null && bookings.Count() > 0)
            {
                bookings = bookings.OrderByDescending(x => x.CreationDate).ToList();

                listGoodsEntity.AddRange(bookings.Select(bk =>
                {
                    var enquiry = enquiries.Where(y => y.EnquiryPID == bk.EnquiryFID).FirstOrDefault();
                    if (enquiry != null)
                    {
                        return new BookingResponseEntity
                        {
                            EnquiryId = Convert.ToInt32(enquiry.EnquiryPID),
                            LoadingCharges = bk.LoadingCharges,
                            UnloadingCharges = bk.UnloadingCharges,
                            PackagingCharges = bk.PackagingCharges,
                            Freight = Convert.ToInt32(bk.Freight),
                            Advance = Convert.ToString(bk.Advance),
                            From = enquiry.From,
                            To = enquiry.To,
                            FromAddress = enquiry.FromAddress,// string.IsNullOrEmpty(enquiry.FromAddress) ? enquiry.From:enquiry.FromAddress ,
                            ToAddress = enquiry.ToAddress,// string.IsNullOrEmpty(enquiry.ToAddress) ? enquiry.To : enquiry.ToAddress,
                            Name = enquiry.Name,
                            MobileNumber = Convert.ToString(enquiry.MobileNumber),
                            MinWeight = Convert.ToInt32(enquiry.MinWeight),
                            MaxWeight = Convert.ToInt32(enquiry.MaxWeight),
                            MaterialType = enquiry.MaterialType.Type + (!string.IsNullOrEmpty(enquiry.Comments) ? " : " + enquiry.Comments : ""),
                            VehicleType = enquiry.VehicleType.Type,
                            //ImgVehicleType = GenericConstant.IMAGES_BASE_ADDRESS + x.VehicleType.Type.Replace(" ", "") + GenericConstant.IMG_EXT,
                            ImgVehicleType = enquiry.VehicleType.Type.ToLower().Replace(" ", ""),
                            VehicleLength = enquiry.VehicleLength,
                            Status = enquiry.Status,
                            ValidTill = enquiry.ExpiryDate,
                            UserId = Convert.ToInt64(enquiry.UserFID),
                            TotalCharges = GetTotalCharges(new List<string> { Convert.ToString(bk.Freight),bk.LoadingCharges,bk.UnloadingCharges,bk.PackagingCharges }),
                            Comments = GetComments(new Dictionary<string, string> { {"Freight", Convert.ToString(bk.Freight) },{ "Loading", bk.LoadingCharges },{ "Unloading", bk.UnloadingCharges },{ "Packaging", bk.PackagingCharges } }),
                        };
                    }
                    else
                    {
                        return new BookingResponseEntity
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
                            Advance = Convert.ToString(bk.Advance),
                            MaterialType = enquiry.MaterialType.Type + (!string.IsNullOrEmpty(enquiry.Comments)? " : "+enquiry.Comments:""),
                            VehicleType = enquiry.VehicleType.Type,
                            //ImgVehicleType = GenericConstant.IMAGES_BASE_ADDRESS + x.VehicleType.Type.Replace(" ", "") + GenericConstant.IMG_EXT,
                            ImgVehicleType = enquiry.VehicleType.Type.ToLower().Replace(" ", ""),
                            VehicleLength = enquiry.VehicleLength,
                            Status = enquiry.Status,
                            ValidTill = enquiry.ExpiryDate,
                            UserId = Convert.ToInt64(enquiry.UserFID),
                            TotalCharges = GetTotalCharges(new List<string> { Convert.ToString(bk.Freight), bk.LoadingCharges, bk.UnloadingCharges, bk.PackagingCharges }),
                            Comments = GetComments(new Dictionary<string, string> { { "Freight", Convert.ToString(bk.Freight) }, { "Loading", bk.LoadingCharges }, { "Unloading", bk.UnloadingCharges }, { "Packaging", bk.PackagingCharges } }),
                        };
                    }
                }
                ));
            }
            return listGoodsEntity;
        }

        private string GetTotalCharges(List<string> charges)
        {
            long sum = 0;
            foreach (var item in charges)
            {
                long price = 0;
                sum = sum + (long.TryParse(item,out price) ? price : 0);
            }

            return Convert.ToString(sum);
        }

        private string GetComments(Dictionary<string,string> charges)
        {
            string comments = "Charges includes ";
            foreach (var item in charges)
            {
                switch(item.Key)
                {
                    case "Freight": comments= comments + (!string.IsNullOrEmpty(item.Value)? "Freight, ":"");
                        break;
                    case "Loading":
                        comments = comments + (!string.IsNullOrEmpty(item.Value) ? "Loading, " : "");
                        break;
                    case "Unloading":
                        comments = comments + (!string.IsNullOrEmpty(item.Value) ? "Unoading, " : "");
                        break;
                    case "Packaging":
                        comments = comments + (!string.IsNullOrEmpty(item.Value) ? "Packaging, " : "");
                        break;
                }
            }

            return comments;
        }
    }
}
