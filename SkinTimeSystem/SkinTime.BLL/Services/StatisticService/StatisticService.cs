using Microsoft.EntityFrameworkCore;
using SkinTime.BLL.Commons;
using SkinTime.BLL.Commons.DTOs.StatisticDTOs;
using SkinTime.DAL.Entities;
using SkinTime.DAL.Enum;
using SkinTime.DAL.Interfaces;

namespace SkinTime.BLL.Services.StatisticService
{
    public class StatisticService : IStatisticService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StatisticService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<Dictionary<string, int>>> GetBookingStatisticByStatus(DateOnly? from, DateOnly? to)
        {
            if (from != null && to != null && from > to)
            {
                return ServiceResult<Dictionary<string, int>>
                    .Failed(ServiceError.ValidationFailed("the 'from' date must not be larger than the 'to' date"));
            }

            DateOnly actualFrom = from ?? DateOnly.MinValue;
            DateOnly actualTo = to ?? DateOnly.MaxValue;

            IEnumerable<Booking> filtered = await _unitOfWork.Repository<Booking>()
                .ListAsync(x => actualFrom <= DateOnly.FromDateTime(x.ReservedTime) && DateOnly.FromDateTime(x.ReservedTime) <= actualTo);

            Dictionary<string, int> results = new Dictionary<string, int>();

            foreach (BookingStatus status in Enum.GetValues(typeof(BookingStatus)))
            {
                results[status.ToString()] = filtered.Count(x => x.Status == status);
            }

            return ServiceResult<Dictionary<string, int>>.Success(results);

            // If the filtered list is empty, nothing will be return! 
            //return ServiceResult<Dictionary<string, int>>
            //    .Success(filtered.GroupBy(x => x.Status).ToDictionary(x => x.Key.ToString(), x => x.Count()));
        }

        public async Task<ServiceResult<ICollection<RevenueDTO>>> GetDailyRevenueStatistics(DateOnly? from, DateOnly? to)
        {
            if (from != null && to != null && from > to)
            {
                return ServiceResult<ICollection<RevenueDTO>>
                    .Failed(ServiceError.ValidationFailed("the 'from' date must not be larger than the 'to' date"));
            }

            DateOnly actualFrom = from ?? DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-6));
            DateOnly actualTo = to ?? DateOnly.FromDateTime(DateTime.UtcNow);

            IEnumerable<Transaction> filtered = await _unitOfWork.Repository<Transaction>()
                .ListAsync(x => x.Include(b => b.BookingNavigation).Include(b => b.TicketNavigation)
                , x => actualFrom <= DateOnly.FromDateTime(x.TransactionTime) && DateOnly.FromDateTime(x.TransactionTime) <= actualTo);

            ICollection<RevenueDTO> results = new List<RevenueDTO>();

            for (DateOnly i = actualFrom; i <= actualTo; i = i.AddDays(1))
            {
                decimal total = 0;
                IDictionary<string, decimal> breakdown = new Dictionary<string, decimal>
                {{"refund", 0}, {"booking", 0 }, {"event", 0}};

                foreach (Transaction t in filtered.Where(x => DateOnly.FromDateTime(x.TransactionTime) == i))
                {
                    total += t.Amount;

                    if (t.IsRefundTransaction)
                    {
                        breakdown["refund"] += t.Amount;
                    }

                    if (t.BookingNavigation != null)
                    {
                        breakdown["booking"] += t.Amount;
                    }

                    if (t.TicketNavigation != null)
                    {
                        breakdown["event"] += t.Amount;
                    }
                }


                results.Add(new RevenueDTO { Date = i, TotalRevenue = total, RevenueBreakDown = breakdown });
            }

            return ServiceResult<ICollection<RevenueDTO>>.Success(results);

            // This will return empty result if filtered list is empty.
            //return ServiceResult<ICollection<RevenueDTO>>.Success(filtered
            //    .GroupBy(x => x.TransactionTime.Date)
            //    .Select(x => new RevenueDTO
            //    {
            //        Date = DateOnly.FromDateTime(x.Key),
            //        TotalRevenue = x.Sum(x => x.Amount),
            //    }).ToList());
        }

        public Task<ServiceResult<Dictionary<string, int>>> GetEventStatisticByStatus(DateOnly? from, DateOnly? to)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<ICollection<Service>>> GetMostPopularService(int limit = 5, bool includeDeleted = false)
        {
            if (limit <= 0)
            {
                return ServiceResult<ICollection<Service>>.Failed(ServiceError.ValidationFailed("limit can not be 0 or a negative number"));
            }

            IEnumerable<Service> filtered = await _unitOfWork.Repository<Service>()
                .ListAsync(x => x.Include(b => b.BookingNavigation), x => x.BookingNavigation.Count() > 0 && (x.Status != ServiceStatus.Deleted || includeDeleted));

            return ServiceResult<ICollection<Service>>
                .Success(filtered.OrderByDescending(x => x.BookingNavigation.Count()).Take(limit).ToList());
        }

        public async Task<ServiceResult<OverviewStatisticsDTO>> GetOverviewStatistics(DateOnly? from, DateOnly? to)
        {
            if (from != null && to != null && from > to)
            {
                return ServiceResult<OverviewStatisticsDTO>
                    .Failed(ServiceError.ValidationFailed("the 'from' date must not be larger than the 'to' date"));
            }

            DateOnly actualFrom = from ?? DateOnly.FromDateTime(DateTime.UtcNow);
            DateOnly actualTo = to ?? DateOnly.FromDateTime(DateTime.UtcNow);

            IEnumerable<Transaction> filtered = await _unitOfWork.Repository<Transaction>()
                .ListAsync(x => x.Include(b => b.BookingNavigation).Include(b => b.TicketNavigation)
                , x => actualFrom <= DateOnly.FromDateTime(x.TransactionTime) && DateOnly.FromDateTime(x.TransactionTime) <= actualTo);

            var servies = await _unitOfWork.Repository<Service>().ListAsync();
            var booking = await _unitOfWork.Repository<Booking>().ListAsync();
            var user = await _unitOfWork.Repository<User>().ListAsync();
            var therapist = await _unitOfWork.Repository<Therapist>().ListAsync();
            var transaction = await _unitOfWork.Repository<Transaction>().ListAsync();

            OverviewStatisticsDTO result = new()
            {
                NewBooking = booking.Count(),
                CompletedBooking = booking.Where(x => x.Status == BookingStatus.Completed).Count(),
                CanceledBooking = booking.Where(x => x.Status == BookingStatus.Canceled).Count(),
                CancelRate = booking.Count() > 0 ? (double)booking.Where(x => x.Status == BookingStatus.Canceled).Count() / booking.Count() * 100 : 0,
                Revenue = transaction.Sum(x => x.Amount),
                NewCustomer = user.Where(x => x.Role == UserRole.Customer && DateOnly.FromDateTime(x.CreatedTime) == DateOnly.FromDateTime(DateTime.UtcNow)).Count(),
                ActiveService = servies.Where(x => x.Status == ServiceStatus.Available).Count(),
                InactiveService = servies.Where(x => x.Status == ServiceStatus.Unavailable).Count(),
                ActiveTherapist = therapist.Where(x => x.Status == TherapistStatus.Available).Count(),
                InactiveTherapist = therapist.Where(x => x.Status == TherapistStatus.Unavailable).Count(),
            };

            return ServiceResult<OverviewStatisticsDTO>.Success(result);
        }
    }
}
