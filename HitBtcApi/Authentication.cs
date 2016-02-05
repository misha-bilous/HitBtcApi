using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HitBtcApi
{
    public class Authentication
    {
        public void Authenticate(string apiKey, string secretKey)
        {
            //var client = new RestClient("https://api.hitbtc.com");

            //var request = new RestRequest("/api/1/trading/balance", Method.GET);
            //request.AddParameter("nonce", GetNonce());
            //request.AddParameter("apikey", apiKey);

            //string sign = CalculateSignature(client.BuildUri(request).PathAndQuery, secretKey);
            //request.AddHeader("X-Signature", sign);

            //var response = client.Execute(request);

            //Console.WriteLine(response.Content);
        }

       
    }
}
