using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;

namespace EronNew.Resources
{
    public interface IGlobalCultureService
    {
        string CurrentCurrency { get; set; }
        bool Premium { get; set; }

        string ConvertCurrency(string from, int amount, string to);
        string GetCurrencySymbol(string currency);
        LocalizedString GetLocalized(string key);
        LocalizedHtmlString GetLocalizedHtml(string key);
    }
}