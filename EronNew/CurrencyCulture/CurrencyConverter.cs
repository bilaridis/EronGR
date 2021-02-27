using EronNew.CurrencyCulture.Model;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System;
using System.Runtime.Caching;

namespace EronNew.CurrencyCulture
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private readonly decimal roundStep = 0M;
        private readonly ObjectCache cache = MemoryCache.Default;
        private readonly CacheItemPolicy policy = new CacheItemPolicy();

        public CurrencyConverter(decimal roundStep = 0M, int secondsToCacheExpire = 3600)
        {
            if (roundStep > 0)
                this.roundStep = roundStep;

            policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(secondsToCacheExpire);
        }

        //public CurrencyConverter(string DataSource)
        //{
        //    DataCollector DataCollector = new DataCollector();
        //    TCMBData = DataCollector.GetTCMBData();

        //    this.DataSourceName = DataSource;
        //}

        private TCMBModel TCMBData
        {
            get
            {
                if (cache.Get("TCMBData") == null)
                {
                    return GetTCMBData();
                }
                else
                {
                    return (TCMBModel)cache.Get("TCMBData");
                }
            }
        }

        private TCMBModel GetTCMBData()
        {

            XmlSerializer serializer = new XmlSerializer(typeof(TCMBModel));
            XmlTextReader reader = new XmlTextReader("http://www.tcmb.gov.tr/kurlar/today.xml");
            var TCMBData = (TCMBModel)serializer.Deserialize(reader);

            cache.Set("TCMBData", TCMBData, policy);

            return TCMBData;

        }


        public decimal GetRate(Currency From, Currency To)
        {
            try
            {
                var fromCurrency = TCMBData.Currencies.Where(c => c.Kod == From.InternationalCode).FirstOrDefault();
                var toCurrency = TCMBData.Currencies.Where(c => c.Kod == To.InternationalCode).FirstOrDefault();


                if (From.InternationalCode == To.InternationalCode)
                    return 1;
                else if (To.InternationalCode == "TRY")
                {
                    if (fromCurrency.BanknoteSelling > 0)
                        return fromCurrency.BanknoteSelling / fromCurrency.Unit;
                    else if (fromCurrency.ForexSelling > 0)
                        return fromCurrency.ForexSelling / fromCurrency.Unit;
                    else
                        return -1;

                }
                else if (From.InternationalCode == "TRY")
                {
                    if (toCurrency.BanknoteSelling > 0)
                        return 1 / (toCurrency.BanknoteSelling / toCurrency.Unit);
                    else if (toCurrency.ForexSelling > 0)
                        return 1 / (toCurrency.ForexSelling / toCurrency.Unit);
                    else
                        return -1;
                }
                else if (From.InternationalCode == "USD")
                {
                    if (toCurrency.CrossRateUSD > 0)
                        return toCurrency.CrossRateUSD;
                    else if (toCurrency.CrossRateOther > 0)
                        return 1 / (toCurrency.CrossRateOther / toCurrency.Unit);
                    else
                        return -1;
                }
                else if (To.InternationalCode == "USD")
                {
                    if (fromCurrency.CrossRateUSD > 0)
                        return 1 / (fromCurrency.CrossRateUSD / fromCurrency.Unit);
                    else if (fromCurrency.CrossRateOther > 0)
                        return fromCurrency.CrossRateOther / fromCurrency.Unit;
                    else
                        return -1;
                }
                else
                {
                    if (fromCurrency.BanknoteSelling > 0 && toCurrency.BanknoteSelling > 0)
                        return (fromCurrency.BanknoteSelling / fromCurrency.Unit) / (toCurrency.BanknoteSelling / toCurrency.Unit);
                    else if (fromCurrency.BanknoteSelling > 0 && fromCurrency.ForexSelling > 0)
                        return (fromCurrency.BanknoteSelling / fromCurrency.Unit) / (toCurrency.ForexSelling / toCurrency.Unit);
                    else if (fromCurrency.ForexSelling > 0 && fromCurrency.BanknoteSelling > 0)
                        return (fromCurrency.ForexSelling / fromCurrency.Unit) / (toCurrency.BanknoteSelling / toCurrency.Unit);
                    else if (fromCurrency.ForexSelling > 0 && fromCurrency.ForexSelling > 0)
                        return (fromCurrency.ForexSelling / fromCurrency.Unit) / (toCurrency.ForexSelling / toCurrency.Unit);
                    else
                        return -1;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public decimal Convert(Currency From, decimal FromAmount, Currency To)
        {
            var rate = GetRate(From, To);
            if (roundStep > 0)
                return Round(rate * FromAmount);
            else
                return rate * FromAmount;
        }

        private decimal Round(decimal d)
        {
            var modReminder = d % roundStep;
            var baseValue = d - modReminder;

            if (modReminder > 0)
                return baseValue + roundStep;
            else
                return baseValue;
        }
    }
}
