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
    /// Offers services for bid specific CRUD operations
    /// </summary>
    public class BidService:IBidService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOTPService _otpService;
      
        /// <summary>
        /// Public constructor.
        /// </summary>
        public BidService(IUnitOfWork unitOfWork,IOTPService otpService)
        {
            _unitOfWork = unitOfWork;
            _otpService = otpService;
        }

        /// <summary>
        /// Creates a bid
        /// </summary>
        /// <param name="bidEntity"></param>
        /// <returns></returns>
        public long AddBid(BusinessEntities.BidRequestEntity bidEntity)
        {
            var bid = new Bid
            {
                Price = bidEntity.Price,
                GoodsFID=bidEntity.GoodsId,
                UserFID = bidEntity.UserId
            };
            _unitOfWork.BidRepository.Insert(bid);
            _unitOfWork.Save();
            return bid.BidPID;
        }

        public List<BusinessEntities.BidResponseEntity> GetAllGoodsBid(int goodsId)
        {
            var bid = _unitOfWork.BidRepository.GetMany(x=>x.GoodsFID==goodsId).ToList();
            if (bid.Any())
            {
                return MapBid(bid);
            }
            return null;
        }

        public List<BusinessEntities.BidResponseEntity> GetAllVehicleBid(int vehicleId)
        {
            var bid = _unitOfWork.BidRepository.GetMany(x => x.GoodsFID == vehicleId).ToList();
            if (bid.Any())
            {
                return MapBid(bid);
            }
            return null;
        }

        private List<BidResponseEntity> MapBid(List<Bid> bid)
        {
            List<BidResponseEntity> listBidEntity  = new List<BidResponseEntity>();
          
            if (bid != null && bid.Count() > 0)
            {
                bid = bid.OrderByDescending(x => x.LastUpdatedDate).ToList();

                listBidEntity.AddRange(bid.Select(x =>
                {
                    return new BidResponseEntity
                    {
                        GoodsId = x.GoodsFID,
                        VehicleId=x.VehicleFID,
                        // make it not null
                        Price=Convert.ToInt32(x.Price),
                      //  UserName=x.u
                        UserId= Convert.ToInt64(x.UserFID)
                    };
                }
                ));
            }
            return listBidEntity;
        }
    }
}
