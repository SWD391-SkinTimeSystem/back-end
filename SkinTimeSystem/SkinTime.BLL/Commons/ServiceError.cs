using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.BLL.Commons
{
    public sealed record ServiceError(string Code, string? Description = null, Dictionary<object, object>? Information = null)
    {
        public const string _NotFound = "NotFoundError";
        public const string _ServiceUnhandledException = "InternalError";
        public const string _ValidationFailed = "ValidationError";
        public const string _AlreadyExisted = "EntityExistedError";
        public const string _NotExisted = "EntityNotExistedError";
        public const string _Unauthorized = "InvalidPermissionError";

        public static readonly ServiceError NoError = new(string.Empty);

        public static ServiceError NotFound(string description) => new ServiceError(_NotFound, description);
        public static ServiceError UnhandledException(string description) => new ServiceError(_ServiceUnhandledException, description);
        public static ServiceError ValidationFailed(string description) => new ServiceError(_ValidationFailed, description);
        public static ServiceError ValidationFailed(string description, Dictionary<object,object> information) => new ServiceError(_ValidationFailed, description, information);
        public static ServiceError Existed(string description) => new ServiceError(_AlreadyExisted, description);
        public static ServiceError NotExisted(string description) => new ServiceError(_NotExisted, description);
        public static ServiceError Unauthorized(string description) => new ServiceError(_Unauthorized, description);
    }
}
