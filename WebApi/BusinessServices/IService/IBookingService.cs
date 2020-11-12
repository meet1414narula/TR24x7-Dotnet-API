using System.Collections.Generic;
using BusinessEntities;

namespace BusinessServices
{
    /// <summary>
    /// Goods Service Contract
    /// </summary>
    public interface IBookingService
    {
        QuotationResponseEntity GetGoods(int goodsId);
        List<BusinessEntities.BookingResponseEntity> GetAllQuotations();

        List<BusinessEntities.BookingResponseEntity> GetAllQuotes(QuoteRequestEntity quotationRequestEntity);
        List<GoodsResponseEntity> GetGoodsByUser(UserEntity userEntity);
        long CreateQuotation(BusinessEntities.BookingRequestEntity goodsEntity);
        bool UpdateGoods(int goodsId,BookingRequestEntity goodsEntity);
        bool DeleteGoods(int goodsId);
    }
}
