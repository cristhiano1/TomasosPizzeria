using TomasosPizzeria.Data.Entities;

namespace TomasosPizzeria.Data.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
