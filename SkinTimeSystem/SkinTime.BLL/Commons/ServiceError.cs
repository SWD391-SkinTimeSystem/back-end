using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.BLL.Commons
{
    public sealed record ServiceError(string Code, string? Description = null)
    {
        public static readonly ServiceError NoError = new(string.Empty);
    }
}
