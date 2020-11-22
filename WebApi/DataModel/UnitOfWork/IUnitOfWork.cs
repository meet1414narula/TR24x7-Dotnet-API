using DataModel.GenericRepository;

namespace DataModel.UnitOfWork
{
    public interface IUnitOfWork
    {
        #region Properties
        GenericRepository<Contact> ContactRepository { get; }
        GenericRepository<RoadLine> RoadLineRepository { get; }
        GenericRepository<Contact_RoadLine_Mapping> ContactRoadLineMappingRepository { get; }
        GenericRepository<Good> GoodsRepository { get; }
        GenericRepository<Enquiry> EnquiryRepository { get; }
        GenericRepository<EnquiryStatu> EnquiryStatusRepository { get; }
        GenericRepository<User> UserRepository { get; }

        GenericRepository<UserAccess> UserAccessRepository { get; }
        GenericRepository<UserType> UserTypeRepository { get; }
        GenericRepository<Token> TokenRepository { get; }
        GenericRepository<OTP> OTPRepository { get; }
        GenericRepository<Error> ErrorRepository { get; }
        GenericRepository<City> CityRepository { get; }
        GenericRepository<MaterialType> MaterialTypeRepository { get; }
        GenericRepository<VehicleType> VehicleTypeRepository { get; }
        GenericRepository<ConfigKeyValue> ConfigKeyValueRepository { get; }
        GenericRepository<Bid> BidRepository { get; }

        GenericRepository<Quotation> QuotationRepository { get; }
       
        GenericRepository<Booking> BookingRepository { get; }
        #endregion

        #region Public methods
        /// <summary>
        /// Save method.
        /// </summary>
        void Save(); 
        #endregion
    }
}