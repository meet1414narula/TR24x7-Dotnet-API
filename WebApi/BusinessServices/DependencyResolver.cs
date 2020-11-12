using System.ComponentModel.Composition;
using DataModel;
using DataModel.UnitOfWork;
using Resolver;

namespace BusinessServices
{
    [Export(typeof(IComponent))]
    public class DependencyResolver : IComponent
    {
        public void SetUp(IRegisterComponent registerComponent)
        {
            registerComponent.RegisterType<IOTPService, OTPServices>();
            registerComponent.RegisterType<IGoodsService, GoodsService>();
            registerComponent.RegisterType<IEnquiryService, EnquiryService>();
            registerComponent.RegisterType<IQuotationService, QuotationService>();
            registerComponent.RegisterType<IBookingService, BookingService>();
            registerComponent.RegisterType<IUserService, UserService>();
            registerComponent.RegisterType<ITokenService, TokenService>();
            registerComponent.RegisterType<IErrorService, ErrorService>();
            registerComponent.RegisterType<IHelperService, HelperService>();
            registerComponent.RegisterType<IBidService, BidService>();
        }
    }
}
