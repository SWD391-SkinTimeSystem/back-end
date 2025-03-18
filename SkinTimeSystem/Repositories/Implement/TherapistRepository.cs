using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implement
{
    public class TherapistRepository : GenericRepository<Therapist>, ITherapistRepository
    {
        public TherapistRepository(ApplicationDbContext context) : base(context) { }

        public async Task<ICollection<Therapist>> GetAllTherapistWithScheduleInformation(Expression<Func<Therapist, bool>> filter)
        {
            IEnumerable<Therapist> results = await ListAsync(x => x
            .Include(b => b.UserNavigation)
            .Include(b => b.BookingNavigation).ThenInclude(b => b.FeedbackNavigation)
            .Include(b => b.BookingNavigation).ThenInclude(b => b.ScheduleNavigation)
            .Include(b => b.CertificationNavigation), filter);

            return results.ToList();
        }

        public async Task<Therapist?> GetTherapistInformationWithId(Guid id)
        {
            return await GetByIdAsync(id, x => x
            .Include(b => b.UserNavigation)
            .Include(b => b.BookingNavigation)
            .ThenInclude(x => x.FeedbackNavigation)
            .Include(b => b.CertificationNavigation));
        }
    }
}
