
using BusinessEntities;
using DataModel;

namespace BusinessServices
{
    public interface IUserService
    {
        long Authenticate(long mobileNumber, string password);
        long Create(RegisterEntity registerEntity);

        long GetUserId(long mobileNumber);
    }
}
