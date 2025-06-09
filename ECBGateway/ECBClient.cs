using ECBGateway.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ECBGateway
{
    public class ECBClient : IECBClient
    {
        private readonly HttpClient _httpClient;
        private const string EcbUrl = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml";

        public ECBClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ECBRate>> GetLatestRatesAsync()
        {
            var response = await _httpClient.GetStringAsync(EcbUrl);
            var document = XDocument.Parse(response);

            var ns = document.Root?.GetDefaultNamespace();

            var cubeRoot = document
                .Descendants(ns + "Cube")
                .Where(e => e.Attribute("time") != null)
                .FirstOrDefault();

            var date = DateTime.Parse(cubeRoot?.Attribute("time")?.Value ?? DateTime.UtcNow.ToString());

            var rates = cubeRoot?.Elements()
                .Select(x => new ECBRate
                {
                    Currency = x.Attribute("currency")?.Value ?? "",
                    Rate = decimal.Parse(x.Attribute("rate")?.Value.Replace(".",",") ?? "0"),
                    Date = date
                }).ToList();

            return rates ?? new List<ECBRate>();
        }
    }
}
