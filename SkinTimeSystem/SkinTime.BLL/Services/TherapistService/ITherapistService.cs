using SkinTime.BLL.Commons;
using SkinTime.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.BLL.Services.TherapistService
{
    public interface ITherapistService
    {
        Task<ServiceResult<Therapist>> GetTherapistWithId(Guid id);
        
        Task<ServiceResult<ICollection<Therapist>>> GetAllTherapist();

        Task<ServiceResult<ICollection<Therapist>>> GetAllTherapist(Expression<Func<Therapist, bool>> predicate);

        /// <summary>
        ///     This method is used to get list of therapists with AVAILABLE status.
        /// </summary>
        /// <returns>
        ///     An asynchronous task that represent the find operation.
        ///     the result of the task is a <seealso cref="ICollection{Therapist}"/>.
        /// </returns>
        Task<ServiceResult<ICollection<Therapist>>> GetAvailableTherapist();

        /// <summary>
        ///     This method is used to get list of therapists with AVAILABLE status and currently does not have any scheduled
        ///     work at the <paramref name="duration"/> minutes span of the requested time.
        /// </summary>
        /// <returns>
        ///     An asynchronous task that represent the find operation.
        ///     the result of the task is a <seealso cref="ICollection{Therapist}"/>.
        /// </returns>
        Task<ServiceResult<ICollection<Therapist>>> GetAvailableTherapist(DateOnly date, TimeOnly startTime, int duration);

        Task<ServiceResult<Guid>> AddTherapist(Therapist therapist);

        Task<ServiceResult<Guid>> UpdateTherapist(Guid id, Therapist information);
    }
}
