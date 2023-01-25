using DemoWebAppMvc.Models;

namespace DemoWebAppMvc.Interface
{
    public interface IDashBoardRepository
    {
        Task<List<Race>> GetAllUserRaces();

        Task<List<Club>> GetAllUserClubs();
    }
}
