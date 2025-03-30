using MercadoSocial.Data;
using MercadoSocial.Models;
using Microsoft.EntityFrameworkCore;

namespace MercadoSocial.Logger
{
    public class LoggerService : ILoggerService
    {

        private readonly BankDbContext _bankDbContext;
        public LoggerService(BankDbContext bankDbContext)
        {
            _bankDbContext = bankDbContext;
        }


        public async Task<List<LoggerModel>> GetLoggers()
        {
            List<LoggerModel> logs = await _bankDbContext.Logger.ToListAsync();
            return logs;
        }

       public async Task<List<LoggerModel>> GetLoggersByUserId(int userId)
        {
            List<LoggerModel> log =  await _bankDbContext.Logger.Where(l => l.User.Id == userId).ToListAsync();
            return log;
        }

        public async Task<LoggerModel> CreateLogger(string title, string description, int? userId)
        {
            LoggerModel log = new LoggerModel()
            {
                Title = title,
                Description = description,
                UserId = userId
            };

             await _bankDbContext.AddAsync(log);
             await _bankDbContext.SaveChangesAsync();
            return log;
        }


    }
}
