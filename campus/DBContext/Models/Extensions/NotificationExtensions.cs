using campus.DBContext.DTO.CourseDTO;
using campus.DBContext.Models;

namespace campus.DBContext.Extensions;

public static class NotificationExtensions
{
    public static NotificationDTO ToNotificationDto(this Notification notification)
    {
        return new NotificationDTO()
        {
            text = notification.Text,
            isImportant = notification.IsImportant,
        };
    }
}