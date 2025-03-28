using AutoMapper;
using BusinessObject.Entities;
using Repositories;
using Repositories.Interface;
using Repositories.UnitOfWork;
using Services.Commons.DTOs.Notification;
using Services.Commons.DTOs.Service;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implement
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task CreateNotificationOfSystem(Guid userId, string message, Guid? aboutId = null) => await _unitOfWork.NotificationRepository.SendSystemNotification(userId, message, aboutId);


        public async Task<NotificationWithCountDTO> GetAllNotification(Guid toUserId, int page, int pageSize)
        {
            PaginationResult<Notification> listNotification = await _unitOfWork.NotificationRepository.GetAllNotification(toUserId, page, pageSize);

            int unreadCount = listNotification.Content.Count(n => !n.IsRead);

            var mappedNotifications = _mapper.Map<List<NotificationAll>>(listNotification.Content);

            return new NotificationWithCountDTO
            {
                NumberUnread = unreadCount,
                Notifications = new PaginationResult<NotificationAll>
                {
                    Content = mappedNotifications,
                    CurrentPage = listNotification.CurrentPage,
                    ItemAmount = listNotification.ItemAmount,
                    PageSize = pageSize
                }
            };
        }

        public async Task MarkMultipleAsRead(List<Guid> notificationIds)=> await _unitOfWork.NotificationRepository.MarkMultipleAsRead(notificationIds);
    }
}
