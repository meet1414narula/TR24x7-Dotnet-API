using System.Collections.Generic;
using BusinessEntities;

namespace BusinessServices
{
    /// <summary>
    /// Bid Service Contract
    /// </summary>
    public interface IBidService
    {
       // BidResponseEntity GetBid(int bidId);
        //List<BidResponseEntity> GetAllBid();
        //List<BidResponseEntity> GetBidByUser(UserEntity userEntity);
        long AddBid(BidRequestEntity bidEntity);
        List<BusinessEntities.BidResponseEntity> GetAllGoodsBid(int goodsId);
        List<BusinessEntities.BidResponseEntity> GetAllVehicleBid(int vehicleId);
    }
}
