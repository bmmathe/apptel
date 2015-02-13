using System;

namespace AppTel.Domain.Services
{
    public interface IErrorService
    {
        void LogError(string applicationName, Exception exception);
    }
}
