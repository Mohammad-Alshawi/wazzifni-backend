using System.Threading.Tasks;
using Abp.Dependency;

namespace Wazzifni.Otp
{
    public interface IOtpService : ITransientDependency
    {
        Task<bool> SendOtpWithWhatsAppAsync(string phoneNumber, string otpCode);

    }
}
