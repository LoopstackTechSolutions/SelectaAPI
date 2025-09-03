namespace SelectaAPI.Repository.Interfaces
{
    public interface IHomeRepository
    {
        Task<HomeRepository> Search(string name);
        Task<HomeRepository> WishList(int id);
        Task<HomeRepository> ForYou(int id);
        Task<HomeRepository> Highlights();
        Task<HomeRepository> Notifications(int id);

    }
}
