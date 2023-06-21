using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Services.Services;

namespace TMS.Services.Interfaces
{
    public interface IMailService
    {
        bool SendMail(MailData mailData);
    }
}
