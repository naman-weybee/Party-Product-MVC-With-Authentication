namespace PartyProduct_Exercise_03.Service
{
    public interface IUserService
    {
        string GetUserId();
        bool IsAuthenticated();
    }
}