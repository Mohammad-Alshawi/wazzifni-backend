using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Wazzifni.Otp.BulkSmsIraq;

namespace Wazzifni.Otp
{
    public class OtpService : IOtpService
    {

        private readonly HttpClient _httpClient;
        private readonly BulkSmsIraqOptions _bulkSmsIraqOptions;

        public OtpService(HttpClient httpClient, BulkSmsIraqOptions bulkSmsIraqOptions)
        {
            _httpClient = httpClient;
            _bulkSmsIraqOptions = bulkSmsIraqOptions;
        }

        public async Task<bool> SendOtpWithWhatsAppAsync(string phoneNumber, string otpCode)
        {
            var requestDto = new
            {
                recipient = phoneNumber,
                sender_id = _bulkSmsIraqOptions.SenderId,
                type = "whatsapp",
                message = otpCode,
                lang = "en"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, _bulkSmsIraqOptions.ApiUrl);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _bulkSmsIraqOptions.ApiKey);
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(JsonSerializer.Serialize(requestDto), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}
