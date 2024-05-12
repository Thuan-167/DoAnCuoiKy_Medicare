using DoctorApointment.ViewModels.System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.Application.System.Users
{
    public interface IEmailService
    {
        Task SendTestEmail(UserEmailOptions userEmailOptions);

        Task SendEmailForEmailConfirmationRegister(UserEmailOptions userEmailOptions);
        Task SendEmailForEmailConfirmation(UserEmailOptions userEmailOptions);

        Task SendEmailForForgotPassword(UserEmailOptions userEmailOptions);

        Task SendEmailChangePassword(UserEmailOptions userEmailOptions);
        Task SendEmailAppoitment(UserEmailOptions userEmailOptions);
        Task SendEmailServiceFee(UserEmailOptions userEmailOptions);
        Task SendEmailCancelAppoitment(UserEmailOptions userEmailOptions);
    }
}
