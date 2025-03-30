using MercadoSocial.Models;
using Microsoft.Extensions.Primitives;

namespace MercadoSocial.Logger
{
    public interface ILoggerService
    {
        Task<List<LoggerModel>> GetLoggersByUserId(int  userId);
        Task<List<LoggerModel>> GetLoggers();
        Task<LoggerModel> CreateLogger(string title, string description, int? userId);
    }
}
