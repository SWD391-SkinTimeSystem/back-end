using BusinessObject.Entities;
using BusinessObject.Enum;
using Repositories;
using Services.Commons;
using Services.Commons.DTOs.Therapist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ITherapistService
    {
        Task<ServiceResult<TherapistDTO>> GetTherapistWithId(Guid id);

        Task<PaginationResult<TherapistDTO>> GetAllTherapist(int page, int pageSize);

        Task<PaginationResult<TherapistDTO>> GetAllTherapistWithStatus(int page, int pageSize, TherapistStatus status);

        /// <summary>
        ///     This method is used to get list of therapists with AVAILABLE status.
        /// </summary>
        /// <returns>
        ///     An asynchronous task that represent the find operation.
        ///     the result of the task is a <seealso cref="ICollection{Therapist}"/>.
        /// </returns>
        Task<PaginationResult<TherapistDTO>> GetAvailableTherapist(int page, int pageSize);

        /// <summary>
        ///     This method is used to get list of therapists with AVAILABLE status and currently does not have any scheduled
        ///     work at the <paramref name="duration"/> minutes span of the requested time.
        /// </summary>
        /// <returns>
        ///     An asynchronous task that represent the find operation.
        ///     the result of the task is a <seealso cref="ICollection{Therapist}"/>.
        /// </returns>
        Task<ICollection<TherapistDTO>> GetAvailableTherapist(DateOnly date, TimeOnly startTime, int duration);

        /// <summary>
        ///     This method is the same as <see cref="GetAvailableTherapist()"/> but will instead return the first available therapist.
        /// </summary>
        /// <param name="date">The date to find the available therapist</param>
        /// <param name="startTime">The time to get the therapist</param>
        /// <param name="duration">The service duration</param>
        /// <returns>
        ///     An asynchronous task that represent the find operation.
        ///     the result of the task is a <seealso cref="Therapist"/>.
        /// </returns>
        Task<ServiceResult<TherapistDTO>> GetFirstAvailableTherapist(DateOnly date, TimeOnly startTime, int duration);

        Task<ServiceResult<Guid>> AddTherapist(Therapist therapist);

        Task<ServiceResult<Guid>> UpdateTherapist(Guid id, Therapist information);
    }
}
