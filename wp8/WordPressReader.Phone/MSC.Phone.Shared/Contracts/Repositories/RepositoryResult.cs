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

        public static RepositoryResult<T> Create(T value, bool isCurrent=true)
        {
            return new RepositoryResult<T>(value, isCurrent);
        }

        public static RepositoryResult<T> CreateError(Exception xcp)
        {
            return new RepositoryResult<T>(xcp);
        }

        public bool IsCurrent { get; protected set; }
        public bool IsError { get; protected set; }
        public Exception Exception { get; protected set; }
        public T Value { get;  protected set; }

        protected RepositoryResult(T value, bool isCurrent)
        {
            Value = value;
            IsCurrent = isCurrent;
        }

        protected RepositoryResult(Exception xcp)
        {
            Exception = xcp;
            IsError = true;
        }

    }
}
