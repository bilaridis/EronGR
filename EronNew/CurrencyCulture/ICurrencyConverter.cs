using EronNew.CurrencyCulture.Model;

namespace EronNew.CurrencyCulture
{
    public interface ICurrencyConverter
    {
        decimal Convert(Currency From, decimal FromAmount, Currency To);
        decimal GetRate(Currency From, Currency To);
    }
}