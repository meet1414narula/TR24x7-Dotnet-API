using System.Collections.Generic;
using BusinessEntities;

namespace BusinessServices
{
    /// <summary>
    /// Goods Service Contract
    /// </summary>
    public interface IEnquiryService
    {
        EnquiryResponseEntity GetGoods(int goodsId);
        List<BusinessEntities.EnquiryResponseEntity> GetAllEnquiries();
        List<GoodsResponseEntity> GetGoodsByUser(UserEntity userEntity);
        long CreateEnquiry(EnquiryRequestEntity goodsEntity);
        bool UpdateGoods(int goodsId,EnquiryRequestEntity goodsEntity);
        bool DeleteGoods(int goodsId);
    }
}
