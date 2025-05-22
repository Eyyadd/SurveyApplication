using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.IService
{
    public interface INotificationService
    {
        Task SendNewPollNotification(int? pollId = null);
    }
}
