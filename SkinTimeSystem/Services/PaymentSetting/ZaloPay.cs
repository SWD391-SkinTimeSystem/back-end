using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Services.PaymentSetting
{
    public class ZaloPay
    {
        public string? AppId { get; set; }
        public string? Key1 { get; set; }
        public string? Key2 { get; set; }
        public string? CreateOrderUrl { get; set; }
        public string? Description { get; set; }
        public string? BankCode { get; set; }
        public double AmountInUsd { get; private set; }
        public string? RefundUrl { get; set; }
        public string? QueryOrderUrl { get; set; }

        #region ZALOPAY
        public async Task<string> CreateZaloPayOrder(decimal? amount, string returnCallBack, string serviceName)
        {
            var response = await CreateZaloPayQrOrderAsync(amount, returnCallBack, serviceName);


            if (response.TryGetValue("order_url", out var orderUrl))
            {
                return orderUrl; 
            }
            throw new Exception("Failed to create ZaloPay order.");
        }
        public async Task<string> CreateZaloPayRefund(decimal? amount, string returnCallBack, string serviceName)
        {
            var response = await CreateZaloPayRefundAsync(amount, returnCallBack, serviceName);


            if (response.TryGetValue("order_url", out var orderUrl))
            {
                return orderUrl; 
            }
            throw new Exception("Failed to create ZaloPay order.");

        }
        #endregion

        #region Request Process
        public async Task<Dictionary<string, string>> CreateZaloPayQrOrderAsync(
            decimal? amount,
            string returnCallBack,
            string serviceName
        )
        {

            Random rnd = new Random();
            var embed_data = new
            {
                redirecturl = returnCallBack,
            };
            var items = new[] { new { } };
            var param = new Dictionary<string, string>();
            var app_trans_id = rnd.Next(1000000);
            param.Add("app_id", AppId);
            param.Add("app_user", "user123");
            param.Add("app_time", GetTimeStamp().ToString());
            param.Add("amount", amount.ToString());
            param.Add("app_trans_id", DateTime.Now.ToString("yyMMdd") + "_" + app_trans_id);
            param.Add("embed_data", JsonConvert.SerializeObject(embed_data));
            param.Add("item", JsonConvert.SerializeObject(items));

            param.Add("description", Description + serviceName);
            param.Add("bank_code", BankCode);
            param.Add("callback_url", returnCallBack);


            var data =
                AppId
                + "|"
                + param["app_trans_id"]
                + "|"
                + param["app_user"]
                + "|"
                + param["amount"]
                + "|"
                + param["app_time"]
                + "|"
                + param["embed_data"]
                + "|"
                + param["item"];
            param.Add("mac", Compute(ZaloPayHMAC.HMACSHA256, Key1, data));

            return await PostFormAsync<Dictionary<string, string>>(CreateOrderUrl, param);
        }
        public async Task<Dictionary<string, string>> CreateZaloPayRefundAsync(
     decimal? amount, string returnCallBack, string serviceName)
        {
            if (amount == null || amount <= 0)
            {
                throw new Exception("Số tiền hoàn phải lớn hơn 0.");
            }

            Random rnd = new Random();
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

            // ✅ LẤY `zptransid` TỪ DB HOẶC GIAO DỊCH TRƯỚC
            var zptransid = "250314000009078"; // ⚠️ Thay bằng ID thực tế của giao dịch

            // ✅ Mã hoàn tiền hợp lệ
            var mrefundid = $"{DateTime.UtcNow:yyMMdd}_{AppId}_{rnd.Next(100000000, 999999999)}";

            // ✅ Định dạng description đúng
            var description = "Hoàn tiền " + serviceName;
            if (description.Length > 100)
            {
                description = description.Substring(0, 100);
            }

            var param = new Dictionary<string, string>
    {
        { "appid", AppId },
        { "mrefundid", mrefundid },
        { "zptransid", zptransid },
        { "amount", ((long)amount).ToString() }, // Phải là số nguyên
        { "timestamp", timestamp },
        { "description", description }
    };

            // ✅ Tạo chữ ký bảo mật đúng format
            var data = $"{AppId}|{zptransid}|{param["amount"]}|{description}|{timestamp}";
            param.Add("mac", Compute(ZaloPayHMAC.HMACSHA256, Key1, data));

            Console.WriteLine("🚀 Gửi yêu cầu hoàn tiền: " + JsonConvert.SerializeObject(param));

            var response = await PostFormAsync<Dictionary<string, string>>("https://sandbox.zalopay.com.vn/v001/tpe/partialrefund", param);
            Console.WriteLine("🛠 Phản hồi từ ZaloPay: " + JsonConvert.SerializeObject(response));

            return response;
        }

        public static long GetTimeStamp(DateTime date)
        {
            return (long)
                (date.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
        }

        public static long GetTimeStamp()
        {
            return GetTimeStamp(DateTime.Now);
        }

        public static string Compute(
            ZaloPayHMAC algorithm = ZaloPayHMAC.HMACSHA256,
            string key = "",
            string message = ""
        )
        {
            byte[] keyByte = Encoding.UTF8.GetBytes(key);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            byte[] hashMessage = null;

            switch (algorithm)
            {
                case ZaloPayHMAC.HMACMD5:
                    hashMessage = new HMACMD5(keyByte).ComputeHash(messageBytes);
                    break;
                case ZaloPayHMAC.HMACSHA1:
                    hashMessage = new HMACSHA1(keyByte).ComputeHash(messageBytes);
                    break;
                case ZaloPayHMAC.HMACSHA256:
                    hashMessage = new HMACSHA256(keyByte).ComputeHash(messageBytes);
                    break;
                case ZaloPayHMAC.HMACSHA512:
                    hashMessage = new HMACSHA512(keyByte).ComputeHash(messageBytes);
                    break;
                default:
                    hashMessage = new HMACSHA256(keyByte).ComputeHash(messageBytes);
                    break;
            }

            return BitConverter.ToString(hashMessage).Replace("-", "").ToLower();
        }

        public enum ZaloPayHMAC
        {
            HMACMD5,
            HMACSHA1,
            HMACSHA256,
            HMACSHA512,
        }

        public static Task<T> PostFormAsync<T>(string uri, Dictionary<string, string> data)
        {
            return PostAsync<T>(uri, new FormUrlEncodedContent(data));
        }

        public static async Task<T> PostAsync<T>(string uri, HttpContent content)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(uri, content);
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseString);
        }
        #endregion
        #region Reponse Process
        public async Task<bool> HandleZaloPayCallback(IQueryCollection data)
        {
            try
            {
                var checksumData =
                    $"{data["appid"]}|{data["apptransid"]}|{data["pmcid"]}|{data["bankcode"]}|{data["amount"]}|{data["discountamount"]}|{data["status"]}";
                var checksum = Compute(ZaloPayHMAC.HMACSHA256, Key2, checksumData);

                if (!checksum.Equals(data["checksum"]))
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error callback ZaloPay: {ex.Message}");
                return false;
            }
        }
        #endregion
    }
}
