using System.Threading.Tasks;
using Abp.Application.Services;
using Wazzifni.Otp.Dto;

namespace Wazzifni.Otp
{
    public interface IOtpService : IApplicationService
    {
        Task<bool> SendOtpWithWhatsAppAsync(string phoneNumber, string otpCode);

        Task<dynamic> SendMessageAsync(SendMessageOtpDto input);

    }
}
