using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Objectia.Api
{
    public class Currency : ApiBase
    {
        private Currency() { }

        public static async Task<CurrencyRate> GetLatestAsync(string baseCurrency = null, string[] currencies = null)
        {
            var query = makeQuery(baseCurrency, currencies);
            var response = await GetJSON("/v1/currency/latest" + query);
            return await ParseResponse<CurrencyRate>(response);
        }

        public static async Task<CurrencyRate> GetHistoricalAsync(DateTime date, string baseCurrency = null, string[] currencies = null)
        {
            var dt = date.ToString("yyyy-MM-dd");
            var query = makeQuery(baseCurrency, currencies);
            var response = await GetJSON($"/v1/currency/historical/{dt}" + query);
            return await ParseResponse<CurrencyRate>(response);
        }
        public static async Task<CurrencyConvert> ConvertAsync(double amount, string from, string to, DateTime? date = null)
        {
            var dt = (date != null) ? date.Value.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
            var response = await GetJSON($"/v1/currency/convert/{amount}/{from}/{to}?date={dt}");
            return await ParseResponse<CurrencyConvert>(response);
        }

        public static async Task<CurrencyChange> GetChangeAsync(DateTime startDate, DateTime endDate, string baseCurrency = null, string[] currencies = null)
        {
            var start = startDate.ToString("yyyy-MM-dd");
            var end = endDate.ToString("yyyy-MM-dd");
            var query = makeQuery(baseCurrency, currencies);
            var response = await GetJSON($"/v1/currency/change/{start}/{end}" + query);
            return await ParseResponse<CurrencyChange>(response);
        }

        public static async Task<CurrencyTimeframe> GetTimeframe(string currency, DateTime startDate, DateTime endDate, string baseCurrency = "USD")
        {
            var start = startDate.ToString("yyyy-MM-dd");
            var end = endDate.ToString("yyyy-MM-dd");
            var response = await GetJSON($"/v1/currency/timeframe/{currency}/{start}/{end}?base={baseCurrency}");
            return await ParseResponse<CurrencyTimeframe>(response);
        }


        private static string makeQuery(string baseCurrency = null, string[] currencies = null)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(baseCurrency))
            {
                sb.Append("?base=" + baseCurrency);
            }
            if (currencies != null)
            {
                sb.Append(sb.Length == 0 ? "?" : "&");
                sb.Append("currencies=" + string.Join(",", currencies));
            }

            return sb.ToString();
        }
    }

    public class CurrencyRate
    {
        [JsonProperty("base")]
        public string Base { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("timestamp")]
        public Int64 Timestamp { get; set; }

        [JsonProperty("rates")]
        public Dictionary<string, double> Rates { get; set; }
    }


    public class CurrencyConvert
    {
        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("timestamp")]
        public Int64 Timestamp { get; set; }

        [JsonProperty("rate")]
        public double Rate { get; set; }

        [JsonProperty("result")]
        public double Result { get; set; }
    }

    public class CurrencyRateChange
    {
        [JsonProperty("start")]
        public double StartRate { get; set; }

        [JsonProperty("end")]
        public double EndRate { get; set; }

        [JsonProperty("change")]
        public double Change { get; set; }

        [JsonProperty("change_percent")]
        public double ChangePercent { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }

    public class CurrencyChange
    {
        [JsonProperty("base")]
        public string Base { get; set; }

        [JsonProperty("start_date")]
        public string StartDate { get; set; }

        [JsonProperty("end_date")]
        public string EndDate { get; set; }

        [JsonProperty("rates")]
        public Dictionary<string, CurrencyRateChange> Rates { get; set; }
    }

    public class CurrencyTimeframe
    {
        [JsonProperty("base")]
        public string Base { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("start_date")]
        public string StartDate { get; set; }

        [JsonProperty("end_date")]
        public string EndDate { get; set; }

        [JsonProperty("timespan")]
        public int Timespan { get; set; }

        [JsonProperty("rates")]
        public Dictionary<string, double> Rates { get; set; }
    }

    public class CurrencyList
    {
        [JsonProperty("currencies")]
        public Dictionary<string, string> Currencies { get; set; }
    }
}


