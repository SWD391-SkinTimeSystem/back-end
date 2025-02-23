using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.BLL.Commons
{
    /// <summary>
    ///     <para>
    ///         This class represent a service layer operation result.
    ///     </para>
    /// </summary>
    public class ServiceResult
    {
        /// <summary>
        /// Whether or not the operation is succeed.
        /// </summary>
        public bool IsSuccess { get; protected set; }

        /// <summary>
        /// Get the error in case of a failure occured, always return <see cref="ServiceError.NoError"/>
        /// if the operation successfully completed.
        /// </summary>
        public ServiceError Error { get; protected set; }

        /// <summary>
        /// Check if the operation is failed.
        /// </summary>
        public bool IsFailed => !IsSuccess;

        /// <summary>
        /// The actual result data.
        /// </summary>
        protected object? _data { get; set; }

        /// <summary>
        /// Get the underlying result data.
        /// </summary>
        virtual public object? Data
        {
            get
            {
                if (IsFailed)
                {
                    throw new InvalidOperationException("Failed operation does not contain result");
                }
                return _data;
            }
            protected set
            {
                _data = value;
            }
        }

        protected ServiceResult(bool success, ServiceError error, object? Data = null)
        {
            if (success && error != ServiceError.NoError || !success && error == ServiceError.NoError)
            {
                throw new ArgumentException("Invalid result argument");
            }
            IsSuccess = success;
            Error = error;
            this.Data = Data;
        }

        public static ServiceResult Success(object? Data = null) => new(true, ServiceError.NoError, Data);

        public static ServiceResult Failed(ServiceError error) => new(false, error);

    }

    /// <summary>
    ///     <para>
    ///         This class inherited from the base class <see cref="ServiceResult"/> supports generic types
    ///         allowing services ability to return the operation result with a strongly typed class. 
    ///     </para>
    /// </summary>
    public sealed class ServiceResult<T> : ServiceResult
    {
        new public T? Data {
            get
            {
                if (IsFailed)
                {
                    throw new InvalidOperationException("Failed operation does not contain result");
                }
                return (T?) _data;
            }
            private set
            {
                _data = value;
            }
        }

        private ServiceResult(bool success, ServiceError error, T? Data = default(T))
        : base(success, error, Data) 
        {
            if (success && error != ServiceError.NoError || !success && error == ServiceError.NoError)
            {
                throw new ArgumentException("Invalid result argument");
            }

            IsSuccess = success;
            Error = error;
            this.Data = Data;
        }

        public static ServiceResult<T> Success(T? Data = default(T)) => new(true, ServiceError.NoError, Data);

        new public static ServiceResult<T> Failed(ServiceError error) => new(false, error);
    }
}
