using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.BLL.Commons
{
    public class ServiceResult<T> where T: class
    {
        public bool IsSuccess { get; }
        public ServiceError Error { get; }
        public T? Result { get; }

        public bool IsFailed => !IsSuccess;

        private ServiceResult(bool success, ServiceError error, T? Data = null) 
        {
            if (success && error != ServiceError.NoError || !success && error == ServiceError.NoError)
            {
                throw new ArgumentException("Invalid result argument");
            }
            IsSuccess = success;
            Error = error;
            Result = Data;
        }

        public static ServiceResult<T> Success(T Data) => new(true, ServiceError.NoError, Data);

        public static ServiceResult<T> Failed(ServiceError error) => new(false, error);
    }
}
