using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System.Threading.Tasks;

namespace Affiliate_Shopee.Services
{
    public class OtpService
    {
        public async Task SendOtpAsync(string fcmToken, string otp)
        {
            
            var message = new Message
            {
                Token = fcmToken,
                Data = new Dictionary<string, string>
                {
                    { "otp", otp }
                }
            };

                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                Console.WriteLine("Gửi thành công: " + response);
            
            //catch (FirebaseMessagingException ex)
            //{
            //    Console.WriteLine("Lỗi khi gửi tin nhắn: " + ex.Message);
            //}
        }

        private string GenerateOtp()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        
    }
}
