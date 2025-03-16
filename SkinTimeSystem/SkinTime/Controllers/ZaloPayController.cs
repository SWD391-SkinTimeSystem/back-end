using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace SkinTime.Controllers
{
    [ApiController]
    [Route("api/zalopay")]
    public class ZaloPayController : ControllerBase
    {
        private const string AppId = "2554"; // Thay bằng AppId của bạn
        private const string Key1 = "sdngKKJmqEMzvh5QQcdD2A9XBSKUNaYn"; // Thay bằng Key1 của bạn
        private const string GetOrderStatusUrl = "https://sb-openapi.zalopay.vn/v2/query";

        private string ComputeHMACSHA256(string key, string data)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        private async Task<Dictionary<string, object>> PostFormAsync(string url, Dictionary<string, string> param)
        {
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(param);
                var response = await client.PostAsync(url, content);
                var json = await response.Content.ReadAsStringAsync();

                Console.WriteLine("Raw Response: " + json);

                return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            }
        }

        [HttpGet("GetZPTransId")]
        public async Task<IActionResult> GetZPTransId(string appTransId)
        {
            try
            {
                if (string.IsNullOrEmpty(appTransId))
                {
                    return BadRequest(new { message = "Thiếu app_trans_id" });
                }

                var param = new Dictionary<string, string>
                {
                    { "app_id", AppId },
                    { "app_trans_id", appTransId }
                };

                // Tạo mac đúng chuẩn ZaloPay
                var data = $"{AppId}|{appTransId}|{Key1}";
                param.Add("mac", ComputeHMACSHA256(Key1, data));

                Console.WriteLine("Request Data: " + JsonConvert.SerializeObject(param, Formatting.Indented));

                // Gọi API kiểm tra trạng thái giao dịch
                var response = await PostFormAsync(GetOrderStatusUrl, param);

                Console.WriteLine("ZaloPay Response: " + JsonConvert.SerializeObject(response, Formatting.Indented));

                // Kiểm tra zp_trans_id
                if (response.ContainsKey("zp_trans_id"))
                {
                    return Ok(new { zp_trans_id = response["zp_trans_id"] });
                }

                return BadRequest(new { message = "Không tìm thấy zptransid", response });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
