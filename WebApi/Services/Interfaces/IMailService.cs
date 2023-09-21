using WebApi.Data;

namespace WebApi.Services.Interfaces
{
    public interface IMailService
    {
        bool SendMail(MailData mailData);
    }
}
