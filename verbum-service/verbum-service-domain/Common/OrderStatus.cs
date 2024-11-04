namespace verbum_service_domain.Common
{
    public enum OrderStatus
    {
        NEW,
        ACCEPTED,
        REJECTED,
        CANCELLED,
        IN_PROGRESS,
        PAID,
        COMPLETED,
        IN_REVIEW
    }
    public static class OrderStatusExtensions
    {
        public static bool IsActive(this string status)
        {
            return status == OrderStatus.NEW.ToString() ||
                   status == OrderStatus.ACCEPTED.ToString() ||
                   status == OrderStatus.IN_PROGRESS.ToString() ||
                   status == OrderStatus.IN_REVIEW.ToString() ||
                   status == OrderStatus.PAID.ToString();
        }
    }
}
