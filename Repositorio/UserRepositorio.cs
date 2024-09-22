using MercadoSocial.Data;
using MercadoSocial.Models;
using MercadoSocial.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MercadoSocial.Repositorio
{
    public class UserRepositorio : IUserRepositorio
    {
        private readonly BankDbContext _dbContext;
        public UserRepositorio(BankDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<List<UserModel>> GetAllUsers()
        {
            List<UserModel> allUsers = await _dbContext.Usuarios.ToListAsync();
            return allUsers;
        }

        public async Task<UserModel> GerUserById(int id)
        {
            UserModel user = await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<UserModel> CreateUser(UserModel user)
        {
            user.DataCriacao = DateTime.Now;
            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<UserModel> UpdateUser(UserModel user)
        {
            UserModel userDB = await GerUserById(user.Id);
            if (user == null)
            {
                throw new Exception("Houve um erro na atualização do usuário.");
            }

            userDB.Name = user.Email;
            userDB.Email = user.Email;
            userDB.Login = user.Login;
            userDB.Perfil = user.Perfil;
            userDB.DataAlteracao = DateTime.Now;

            _dbContext.Update(userDB);
            await _dbContext.SaveChangesAsync();
            return userDB;
        }


        public async Task<bool> DeleteUser(int id)
        {
            UserModel userDB = await GerUserById(id);
            if (userDB == null)
            {
                throw new Exception("Houve um erro ao tentar excluir o usuário.");
            }

            _dbContext.Usuarios.Remove(userDB);
            await _dbContext.SaveChangesAsync();
            return true;
        }


    }
  }
