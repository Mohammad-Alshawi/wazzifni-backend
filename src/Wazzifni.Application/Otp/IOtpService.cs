using System.Threading.Tasks;
using Abp.Application.Services;

namespace Wazzifni.Otp
{
    public interface IOtpService : IApplicationService
    {
        Task<bool> SendOtpWithWhatsAppAsync(string phoneNumber, string otpCode);

    }
}
