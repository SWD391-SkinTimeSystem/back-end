using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace Cursus.Core.Options.PaymentSetting
{
    public class VNPay
    {
        public string? BaseUrl { get; set; }
        public string? TmnCode { get; set; }
        public string? HashSecret { get; set; }
        public string? Version { get; set; }
        public string? Command { get; set; }
        public string? CurrCode { get; set; }
        public string? Locale { get; set; }
        public double AmountInUsd { get; private set; }

        private SortedList<string, string> _requestData = new SortedList<string, string>(
            new VnPayCompare()
        );
        private SortedList<string, string> _responseData = new SortedList<string, string>(
            new VnPayCompare()
        );



        #region VNPAY
        public async Task<string> CreateVNPayOrder(decimal? amount, string returnUrl)
        {
            string ipAddress = await GetIpAddress();
            await ConfigureRequest(amount, returnUrl, ipAddress);
            return await CreatePaymentUrlAsync();
        }
        #endregion

       

        #region Request process
        public async Task ConfigureRequest(decimal? amount, string returnUrl, string ipAddress)
        {
            _requestData.Clear();

            // Thêm đầy đủ các tham số vào _requestData
            AddRequestData("vnp_Version", Version);
            AddRequestData("vnp_Command", Command);
            AddRequestData("vnp_TmnCode", TmnCode);
            AddRequestData("vnp_Amount", (amount * 100).ToString()); // Đã sửa lại đây
            AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            AddRequestData("vnp_CurrCode", CurrCode);
            AddRequestData("vnp_IpAddr", ipAddress);
            AddRequestData("vnp_Locale", Locale);
            AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + Guid.NewGuid().ToString());
            AddRequestData("vnp_OrderType", "other");
            AddRequestData("vnp_ReturnUrl", returnUrl);
            AddRequestData("vnp_TxnRef", Guid.NewGuid().ToString());
        }

        public async Task<string> CreateRequestUrl(string baseUrl, string vnp_HashSecret)
        {
            StringBuilder data = new StringBuilder();

            // Kiểm tra _requestData
            foreach (var item in _requestData)
            {
                Console.WriteLine(item);
            }

            foreach (KeyValuePair<string, string> kv in _requestData)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    data.Append(
                        WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&"
                    );
                }
                else
                {
                    Console.WriteLine($"Key: {kv.Key} has an empty or null value.");
                }
            }

            string queryString = data.ToString().TrimEnd('&');
            baseUrl += "?" + queryString;

            string vnp_SecureHash = await HmacSHA512Async(vnp_HashSecret, queryString);
            return baseUrl + "&vnp_SecureHash=" + vnp_SecureHash;
        }

        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value) && !_requestData.ContainsKey(key))
            {
                _requestData.Add(key, value);
            }
        }

        public async Task<string> CreatePaymentUrlAsync()

        {
            if (string.IsNullOrEmpty(BaseUrl))
                throw new Exception("VNPay BaseUrl is null!");
            if (string.IsNullOrEmpty(HashSecret))
                throw new Exception("VNPay HashSecret is null!");

            return await CreateRequestUrl(BaseUrl, HashSecret);
        }

        public async Task<string> GetIpAddress()
        {
            IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();
            var context = httpContextAccessor.HttpContext;
            return context?.Connection.RemoteIpAddress?.ToString() ?? "Invalid IP";
        }
        #endregion
        #region Response Process
        public void AddResponseData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _responseData.Add(key, value);
            }
        }

        public async Task<bool> ValidateSignature(string inputHash, string secretKey)
        {
            string rspRaw = GetResponseData();
            string myChecksum = await HmacSHA512Async(secretKey, rspRaw);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }

        public async Task<double> GetAmount() => AmountInUsd;

        private string GetResponseData()
        {
            StringBuilder data = new StringBuilder();
            if (_responseData.ContainsKey("vnp_SecureHashType"))
            {
                _responseData.Remove("vnp_SecureHashType");
            }
            if (_responseData.ContainsKey("vnp_SecureHash"))
            {
                _responseData.Remove("vnp_SecureHash");
            }
            foreach (KeyValuePair<string, string> kv in _responseData)
            {
                if (!String.IsNullOrEmpty(kv.Value))
                {
                    data.Append(
                        WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&"
                    );
                }
            }
            //remove last '&'
            if (data.Length > 0)
            {
                data.Remove(data.Length - 1, 1);
            }
            return data.ToString();
        }

        public void AddResponseDataFromQueryString(IQueryCollection query)
        {
            _responseData.Clear();
            foreach (var key in query.Keys)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    AddResponseData(key, query[key]);
                }
            }
        }
        #endregion
        #region Library
        public static async Task<string> HmacSHA512Async(string key, string inputData)
        {
            return await Task.Run(() =>
            {
                var hash = new StringBuilder();
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);

                using (var hmac = new HMACSHA512(keyBytes))
                {
                    byte[] hashValue = hmac.ComputeHash(inputBytes);
                    foreach (var theByte in hashValue)
                    {
                        hash.Append(theByte.ToString("x2"));
                    }
                }
                return hash.ToString();
            });
        }

        public class VnPayCompare : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                return string.Compare(x, y, StringComparison.Ordinal);
            }
        }
        #endregion
    }



  
}
