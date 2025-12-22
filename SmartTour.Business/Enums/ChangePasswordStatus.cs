using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTour.Business.Enums
{
    public enum ChangePasswordStatus
    {
        Success,
        WrongPassword,
        PasswordUnchanged,
        UserNotFound,
        GoogleAccount,
    }
}
