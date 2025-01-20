using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.DAL.Entities
{
    public class BaseEntity
    {// class này chỉ đơn giản chứa id và tất cả các entity khác sẽ kế thừa nó. tại vì tất các entity đều có id
        public Guid Id { get; set; }
    }
}
