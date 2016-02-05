using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using HitBtcApi.Categories;
using RestSharp;

namespace HitBtcApi
{
    public class HitBtcApi
    {
        private const string url = "http://api.hitbtc.com";

        public MarketData MarketData { get; set; }
        public Payment Payment { get; set; }
        public Trading Trading { get; set; }

        public HitBtcApi()
        {
            MarketData = new MarketData(this);
            Payment = new Payment(this);
            Trading = new Trading(this);
        }

        public ApiResponse Execute(RestRequest request, bool requireAuthentication = true)
        {
            if (requireAuthentication && !IsAuthorized)
                throw new Exception("AccessTokenInvalid");

            var client = new RestClient(url);

            if (requireAuthentication)
            {
                request.AddParameter("nonce", GetNonce());
                request.AddParameter("apikey", _apiKey);
                string sign = CalculateSignature(client.BuildUri(request).PathAndQuery, _secretKey);
                request.AddHeader("X-Signature", sign);
            }

            var response = client.Execute(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var exception = new ApplicationException(message, response.ErrorException);
                throw exception;
            }

            return new ApiResponse { content = response.Content };
        }

        #region Authentication

        public bool IsAuthorized { get; set; }

        public void Authorize(string apiKey, string secretKey)
        {
            _apiKey = apiKey;
            _secretKey = secretKey;
            IsAuthorized = true;
        }

        private string _apiKey;
        private string _secretKey;

        private static long GetNonce()
        {
            // use millisecond timestamp or whatever you want
            return DateTime.Now.Ticks * 10 / TimeSpan.TicksPerMillisecond;
        }

        private static string CalculateSignature(string text, string secretKey)
        {
            using (var hmacsha512 = new HMACSHA512(Encoding.UTF8.GetBytes(secretKey)))
            {
                hmacsha512.ComputeHash(Encoding.UTF8.GetBytes(text));

                // minimalistic hex-encoding and lower case
                return string.Concat(hmacsha512.Hash.Select(b => b.ToString("x2")).ToArray());
            }
        }

        #endregion
    }
}
