using BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface ITherapistRepository: IGenericRepository<Therapist>
    {
        Task<Therapist?> GetTherapistInformationWithId(Guid id);

        Task<ICollection<Therapist>> GetAllTherapistWithScheduleInformation(Expression<Func<Therapist, bool>> filter);
    }
}
