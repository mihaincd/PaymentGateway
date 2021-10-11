using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Application.Services
{
    public class DateTimeService
    {
        public static DateTime GetDate() => DateTime.UtcNow;
    }
}
