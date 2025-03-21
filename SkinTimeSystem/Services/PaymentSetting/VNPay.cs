using BusinessObject.Entities;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace Services.PaymentSetting
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
        public string? RefundUrl { get; private set; }

        private SortedList<string, string> _requestData = new SortedList<string, string>(
            new VnPayCompare()
        );
        private SortedList<string, string> _responseData = new SortedList<string, string>(
            new VnPayCompare()
        );

        private readonly HttpClient _httpClient = new HttpClient();

        #region VNPAY
        public async Task<string> CreateVNPayOrder(decimal? amount, string returnUrl,string serviceName)
        {
            string ipAddress = await GetIpAddress();
            await ConfigureRequest(amount, returnUrl, ipAddress, serviceName);
            return await CreateRequestUrl(BaseUrl, HashSecret);
        }

        public async Task<string> CreateVnPayRefund(Transaction transaction) 
        {
            string ipAddress = await GetIpAddress();
            await ConfigureRefundRequest( ipAddress, transaction);
          return await  CreateRequestRefundUrl(RefundUrl, HashSecret);
        }
        #endregion


        #region Request process
        public async Task ConfigureRequest(decimal? amount, string returnUrl, string ipAddress, string serviceName)
        {
            _requestData.Clear();
            AddRequestData("vnp_Version", Version);
            AddRequestData("vnp_Command", Command);
            AddRequestData("vnp_TmnCode", TmnCode);
            AddRequestData("vnp_Amount", (amount * 100).ToString()); 
            AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            AddRequestData("vnp_CurrCode", CurrCode);
            AddRequestData("vnp_IpAddr", ipAddress);
            AddRequestData("vnp_Locale", Locale);
            AddRequestData("vnp_OrderInfo", "Thanh toan dich vu"); 
            AddRequestData("vnp_OrderType", "other");
            AddRequestData("vnp_ReturnUrl", returnUrl);
            AddRequestData("vnp_TxnRef", Guid.NewGuid().ToString());
        }

        public async Task ConfigureRefundRequest(string ipAddress,Transaction transaction)
        {
            _requestData.Clear();
            string transaction_id = transaction.TransactionReference.ToString(); 
            decimal amount = transaction.Amount; 
            
            string transactionDate = transaction.PayDate; 

            string requestId = Guid.NewGuid().ToString("N");

            AddRequestData("vnp_RequestId", requestId);
            AddRequestData("vnp_Version", Version);
            AddRequestData("vnp_Command", "refund");
            AddRequestData("vnp_TmnCode", TmnCode);
            AddRequestData("vnp_TransactionType", "02");

            AddRequestData("vnp_TxnRef", transaction_id);
            AddRequestData("vnp_Amount", ((long)(amount * 100)).ToString());
            AddRequestData("vnp_TransactionDate", transactionDate);

            AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            AddRequestData("vnp_CurrCode", CurrCode);
            AddRequestData("vnp_IpAddr", ipAddress);
            AddRequestData("vnp_Locale", Locale);
            AddRequestData("vnp_OrderInfo", "Thanh toán hoàn tiền");
            AddRequestData("vnp_OrderType", "hoàn tiền");

            string rawData = $"{requestId}|{Version}|refund|{TmnCode}|02|{transaction_id}|{(int)(amount * 100)}||{transactionDate}|{DateTime.Now:yyyyMMddHHmmss}|{ipAddress}|Thanh";
            string secureHash = GenerateSecureHash(rawData);
            AddRequestData("vnp_SecureHash", secureHash);
        }


        private string GenerateSecureHash(string data)
        {
            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes("FE59QM7PYFU533HUBLS2WO9K3ABN3J8I")))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();
            }
        }


        public async Task<string> CreateRequestUrl(string baseUrl, string vnp_HashSecret)
        {
            StringBuilder data = new StringBuilder();

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

        public async Task<string> CreateRequestRefundUrl(string baseUrl, string vnp_HashSecret)
        {
            var data = _requestData.Where(kv => !string.IsNullOrEmpty(kv.Value))
                                   .OrderBy(kv => kv.Key)
                                   .ToDictionary(kv => kv.Key, kv => kv.Value);

            string rawData = string.Join("|", new List<string>
        {
            data.GetValueOrDefault("vnp_RequestId", ""),
            data.GetValueOrDefault("vnp_Version", ""),
            data.GetValueOrDefault("vnp_Command", ""),
            data.GetValueOrDefault("vnp_TmnCode", ""),
            data.GetValueOrDefault("vnp_TransactionType", ""),
            data.GetValueOrDefault("vnp_TxnRef", ""),
            data.GetValueOrDefault("vnp_Amount", ""),
            data.GetValueOrDefault("vnp_TransactionNo", ""),  
            data.GetValueOrDefault("vnp_TransactionDate", ""),
            data.GetValueOrDefault("vnp_CreateDate", ""),
            data.GetValueOrDefault("vnp_IpAddr", ""),
            data.GetValueOrDefault("vnp_OrderInfo", "")
        });

            string vnp_SecureHash = await HmacSHA512Async(vnp_HashSecret, rawData);
            data["vnp_SecureHash"] = vnp_SecureHash;

            string jsonData = JsonSerializer.Serialize(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(baseUrl, content);

            string responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }



        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value) && !_requestData.ContainsKey(key))
            {
                _requestData.Add(key, value);
            }
        }


        public async Task<string> GetIpAddress()
        {
            IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();
            var context = httpContextAccessor.HttpContext;
            return context?.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
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
