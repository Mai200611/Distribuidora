using Distribuidora.Shared.Responses;

namespace Distribuidora.API.Helpers
{
    public interface IMailHelper
    {
        Response SendMail(string toName, string toEmail, string subject, string body);
    }
}
