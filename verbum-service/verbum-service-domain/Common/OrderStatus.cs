namespace verbum_service_domain.Common
{
    public enum OrderStatus
    {
        NEW,
        ACCEPTED,
        REJECTED,
        CANCELLED,
        DEPOSITED,
        PAID
    }
    public static class OrderStatusExtensions
    {
        public static bool IsActive(this string status)
        {
            return status == OrderStatus.NEW.ToString() ||
                   status == OrderStatus.ACCEPTED.ToString() ||
                   status == OrderStatus.DEPOSITED.ToString() ||
                   status == OrderStatus.PAID.ToString();
        }
    }
}
