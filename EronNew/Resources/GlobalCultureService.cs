using EronNew.CurrencyCulture;
using EronNew.CurrencyCulture.Model;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using System.Reflection;

namespace EronNew.Resources
{
    public class GlobalCultureService : IGlobalCultureService
    {
        private readonly IStringLocalizer _localizer;

        private readonly IHtmlLocalizer _htmlLocalizer;

        private readonly CurrencyConverter _currencyConverter;

        public bool Premium { get; set; }

        public string CurrentCurrency { get; set; }

        public GlobalCultureService(IStringLocalizerFactory factory, IHtmlLocalizerFactory htmlFactory)
        {
            _currencyConverter = new CurrencyConverter(0M, 3600);
            var type = typeof(SharedResources);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create("SharedResources", assemblyName.Name);
            _htmlLocalizer = htmlFactory.Create("SharedResources", assemblyName.Name);
        }

        public LocalizedString GetLocalized(string key)
        {
            return _localizer[key];
        }

        public LocalizedHtmlString GetLocalizedHtml(string key)
        {
            return _htmlLocalizer[key];
        }

        public string ConvertCurrency(string from, int amount, string to)
        {
            var _from = new Currency(from);
            var _to = new Currency(to);
            //var rate = _currencyConverter.GetRate(Currency.TRY, Currency.USD);
            var exchange = _currencyConverter.Convert(_from, amount, _to);
            return exchange.ToString("N") + " " + _to.Symbol;
        }

        public string GetCurrencySymbol(string currency)
        {
            var _from = new Currency(currency);
            return _from.Symbol;
        }
    }
}

