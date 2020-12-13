using System.Collections.Generic;
using BusinessEntities;

namespace BusinessServices
{
    /// <summary>
    /// Goods Service Contract
    /// </summary>
    public interface IBookingService
    {
        QuotationResponseEntity GetBooking(int goodsId);
        List<BusinessEntities.BookingResponseEntity> GetAllBookings(int userId);

        List<BusinessEntities.BookedVehicleResponseEntity> GetAllBookedVehicles(int userId);

        List<BusinessEntities.BookingResponseEntity> GetAllBookings();

        List<BusinessEntities.BookingResponseEntity> GetAllBookings(QuoteRequestEntity quotationRequestEntity);
        List<GoodsResponseEntity> GetBookingsByUser(UserEntity userEntity);
        long CreateBooking(BusinessEntities.BookingRequestEntity goodsEntity);
        bool UpdateBooking(int goodsId,BookingRequestEntity goodsEntity);
        bool DeleteBooking(int goodsId);

        int AddVehicle(BusinessEntities.VehicleRequestEntity vehicleEntity);
    }
}
