using MSC.Phone.Shared.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSC.Phone.Shared.Contracts.Repositories
{
    public class RepositoryResult<T>
    {
        public static implicit operator RepositoryResult<T>(T value)
        {
            return RepositoryResult<T>.Create(value);
        }

        public static implicit operator T(RepositoryResult<T> result)
        {
            return result.Value;
        }

        public static implicit operator RepositoryResult<T>(ServiceResult<T> serviceResult)
        {
            return RepositoryResult<T>.Create(
                serviceResult.Value,
                true,
                serviceResult.Successful,
                serviceResult.ErrorCode,
                serviceResult.ErrorMessage
                );
        }

        public static RepositoryResult<T> Create(T value, bool isCurrent = true, bool successful = true, int errorCode = 0, string errorMessage = null)
        {
            return new RepositoryResult<T>(value, isCurrent, successful, errorCode, errorMessage);
        }

        public static RepositoryResult<T> CreateError(Exception xcp)
        {
            return new RepositoryResult<T>(xcp);
        }

        public bool IsCurrent { get; protected set; }
        public Exception Exception { get; protected set; }

        public T Value { get; protected set; }
        public bool Successful { get; protected set; }
        public int ErrorCode { get; protected set; }
        public string ErrorMessage { get; protected set; }

        protected RepositoryResult(T value, bool isCurrent, bool successful, int errorCode, string errorMessage)
        {
            Value = value;
            IsCurrent = isCurrent;
            Successful = successful;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        protected RepositoryResult(Exception xcp)
        {
            Exception = xcp;
            Successful = false;
        }
    }
}
