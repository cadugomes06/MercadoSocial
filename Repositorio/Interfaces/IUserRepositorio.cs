using MercadoSocial.Models;

namespace MercadoSocial.Repositorio.Interfaces
{
    public interface IUserRepositorio
    {
        Task<List<UserModel>> GetAllUsers();
        Task<UserModel> GerUserById(int id);
        Task<UserModel> CreateUser(UserModel user);
        Task<UserModel> UpdateUser(UserModel user);
        Task<bool> DeleteUser(int id);

        Task<UserModel> SearchUserLogin(string login);

    }
}
