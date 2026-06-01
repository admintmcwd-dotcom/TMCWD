using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace TMCWD.Model.Extensions
{
    public static class JobOrderStatusExtension
    {

        public static string GetDescription(this JobOrderStatus status)
        {
            return status switch
            {
                JobOrderStatus.Inspection => "For Inspection",
                JobOrderStatus.Payment => "For Payment",
                JobOrderStatus.Releasing => "For Materials Releasing",
                JobOrderStatus.Charging => "Applying Fees",
                JobOrderStatus.Installation => "For Installation",
                JobOrderStatus.Verification => "For Verification",
                JobOrderStatus.Completed => "Completed",
                JobOrderStatus.Rejected => "Rejected",
                _ => throw new IndexOutOfRangeException(nameof(status))
            };
        }

        public static JobOrderStatus GetNext(this JobOrderStatus status)
        {
            List<JobOrderStatus> statuses = new List<JobOrderStatus>()
            {
                JobOrderStatus.Inspection,
                JobOrderStatus.Charging,
                JobOrderStatus.Payment,
                JobOrderStatus.Releasing,
                JobOrderStatus.Installation,
                JobOrderStatus.Verification,
                JobOrderStatus.Completed,
                JobOrderStatus.Rejected
            };

            var ordered = statuses.Order();

            var nextStatus = statuses.Where(x => (int)x > (int)status).FirstOrDefault();

            return nextStatus;
        }

        public static JobOrderStatus GetNextSkip(this JobOrderStatus status, int skip)
        {
            List<JobOrderStatus> statuses = new List<JobOrderStatus>()
            {
                JobOrderStatus.Inspection,
                JobOrderStatus.Charging,
                JobOrderStatus.Payment,
                JobOrderStatus.Releasing,
                JobOrderStatus.Installation,
                JobOrderStatus.Verification,
                JobOrderStatus.Completed,
                JobOrderStatus.Rejected
            };

            var ordered = statuses.Order();

            var nextStatus = statuses.Where(x => (int)x + skip > (int)status).FirstOrDefault();

            return nextStatus;
        }
    }
}
