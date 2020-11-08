using System.Collections.Generic;
using BusinessEntities;

namespace BusinessServices
{
    /// <summary>
    /// Goods Service Contract
    /// </summary>
    public interface IQuotationService
    {
        QuotationResponseEntity GetGoods(int goodsId);
        List<BusinessEntities.QuotationResponseEntity> GetAllEnquiries();
        List<GoodsResponseEntity> GetGoodsByUser(UserEntity userEntity);
        long CreateQuotation(BusinessEntities.QuotationRequestEntity goodsEntity);
        bool UpdateGoods(int goodsId,EnquiryRequestEntity goodsEntity);
        bool DeleteGoods(int goodsId);
    }
}
