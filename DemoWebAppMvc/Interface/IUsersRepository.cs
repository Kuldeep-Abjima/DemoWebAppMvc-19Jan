using DemoWebAppMvc.Models;

namespace DemoWebAppMvc.Interface
{
    public interface IUsersRepository
    {
        Task<IEnumerable<AppUser>> GetAllUsers();
        Task<AppUser> GetUsersById(string id);

        bool Add(AppUser user);
        bool Update(AppUser user);
        bool Delete(AppUser user);
        bool Save();
    }
}
