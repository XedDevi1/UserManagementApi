namespace UserManagementAPI.Services.Interfaces
{
    public interface IUserDeletionService
    {
        Task DeleteUser(int id);
    }
}
