using API.ViewModel;

namespace API.Helper.Interfaces
{
    public interface IJWTHelper
    {
        string GenerateToken(CredsPayload payload);
    }
}
