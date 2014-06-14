using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSC.Phone.Shared.Contracts.Services
{
    public class ServiceResult<T>
    {
        public static implicit operator ServiceResult<T>(T value)
        {
            return ServiceResult<T>.Create(value);
        }

        public static implicit operator T(ServiceResult<T> result)
        {
            return result.Value;
        }

        public static ServiceResult<T> Create(T value, bool successful = true, int errorCode = 0, string errorMessage = null)
        {
            return new ServiceResult<T>(value, successful, errorCode, errorMessage);
        }

        public bool Successful { get; protected set; }
        public int ErrorCode { get; protected set; }
        public string ErrorMessage { get; protected set; }
        public T Value { get; protected set; }

        protected ServiceResult(T value, bool successful, int errorCode, string errorMessage)
        {
            Value = value;
            Successful = successful;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}
