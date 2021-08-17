using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Models;

namespace ApplicationCore.ServiceInterfaces
{
    public interface IUserService
    {
        Task<UserLoginResponseModel> Login(UserLoginRequestModel requestModel);
        Task<UserRegisterResponseModel> Register(UserRegisterRequestModel requestModel);

        Task<IEnumerable<MovieCardResponseModel>> GetPurchasedMovies(int userId);

        Task<IEnumerable<MovieCardResponseModel>> GetFavorites(int userId);

    }
}