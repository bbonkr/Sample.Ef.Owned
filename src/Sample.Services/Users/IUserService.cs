
namespace Sample.Services.Users
{
    public interface IUserService
    {
        Task<IEnumerable<UserModel>> GetUsersAsync(int page = 1, int limit = 10, string? keyword = null);

        Task<IEnumerable<UserModel>> GetUsersOtherWayAsync(int page = 1, int limit = 10, string? keyword = null);
    }
}