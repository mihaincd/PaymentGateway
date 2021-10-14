using System;

namespace PaymentGateway.Application.Services
{
    public class DateTimeService
    {
        public static DateTime GetDate() => DateTime.UtcNow;
    }
}
